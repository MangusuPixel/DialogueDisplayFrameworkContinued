using FarmerPortraits;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using DialogueDisplayFramework.Api;
using DialogueDisplayFramework.Data;

namespace DialogueDisplayFramework
{
    /// <summary>The mod entry point.</summary>
    public partial class ModEntry : Mod
    {
        public static IMonitor SMonitor;
        public static IModHelper SHelper;
        public static ModConfig Config;
        public static ModEntry context;

        public static string dictPath = "aedenthorn.DialogueDisplayFramework/dictionary";
        public static IAssetName dictAssetName;
        public static string defaultKey = "default";

        public static Dictionary<string, Texture2D> imageDict = new Dictionary<string, Texture2D>();

        private static int validationDelay = 5;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = Helper.ReadConfig<ModConfig>();

            context = this;

            SMonitor = Monitor;
            SHelper = helper;

            dictAssetName = helper.GameContent.ParseAssetName(dictPath);

            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
            helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked_PostCP;

            helper.Events.Content.AssetRequested += Content_AssetRequested;
            helper.Events.Content.AssetRequested += Content_AssetRequested_Post; // After CP edits

            var harmony = new Harmony(ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Constructor(typeof(DialogueBox), new Type[] { typeof(Dialogue) }),
                postfix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(DialogueBoxPatches.Dialogue_Postfix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.gameWindowSizeChanged)),
                postfix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(DialogueBoxPatches.GameWindowSizeChanged_Postfix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.drawPortrait)),
                prefix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(DialogueBoxPatches.DrawPortrait_Prefix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.getCurrentString)),
                prefix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(DialogueBoxPatches.GetCurrentString_Prefix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.draw), new Type[] { typeof(SpriteBatch) }),
                postfix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(DialogueBoxPatches.Draw_Postfix))
            );


        }
        
        public override object GetApi()
        {
            return DialogueDisplayApi.Instance;
        }

        private void Content_AssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (!Config.EnableMod)
                return;

            if (e.NameWithoutLocale.IsEquivalentTo(dictAssetName))
            {
                e.LoadFrom(() => new Dictionary<string, DialogueDisplayData>
                {
                    { defaultKey, DisplayDataHelper.DefaultValues }
                }, AssetLoadPriority.Exclusive);
            }
        }

        [EventPriority(EventPriority.Low)]
        private void Content_AssetRequested_Post(object sender, AssetRequestedEventArgs e)
        {
            if (!Config.EnableMod)
                return;

            if (e.NameWithoutLocale.IsEquivalentTo(dictAssetName))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, DialogueDisplayData>().Data;
                    var hasModsWithMissingID = false;

                    imageDict.Clear();

                    foreach (var (key, entry) in data)
                    {
                        if (entry.Disabled)
                            continue;

                        // Validate copy references

                        if (entry.CopyFrom != null)
                        {
                            var target = entry;
                            var traceStack = new List<DialogueDisplayData>() { entry };
                            var cyclicRef = false;

                            while (!cyclicRef && target.CopyFrom != null && data.TryGetValue(target.CopyFrom, out target))
                            {
                                foreach (var traceStep in traceStack)
                                {
                                    if (traceStep == target)
                                    {
                                        SMonitor.Log($"{key} > {traceStack.Select(d => d.CopyFrom).Join(delimiter: " > ")} : Cyclic reference detected. Disabling.", LogLevel.Error);

                                        foreach (var d in traceStack)
                                            d.Disabled = true;

                                        cyclicRef = true;
                                        break;
                                    }
                                }
                                traceStack.Add(target);
                            }

                            if (!data.ContainsKey(entry.CopyFrom))
                            {
                                SMonitor.Log($"{key} : CopyFrom key points to a non-existant entry: {entry.CopyFrom}", LogLevel.Warn);
                            }
                        }

                        foreach (var image in entry.Images)
                        {
                            if (!imageDict.ContainsKey(image.TexturePath))
                                imageDict[image.TexturePath] = Game1.content.Load<Texture2D>(image.TexturePath);
                        }

                        if (entry.Portrait?.TexturePath != null && !imageDict.ContainsKey(entry.Portrait.TexturePath))
                        {
                            imageDict[entry.Portrait.TexturePath] = Game1.content.Load<Texture2D>(entry.Portrait.TexturePath);
                        }

                        var imagesWithMissingID = entry.Images.Select(i => (!i.Disabled && (i.ID == null || i.ID == DisplayDataHelper.MISSING_ID_STR)) ? 1 : 0).Sum();
                        if (imagesWithMissingID > 0)
                            SMonitor.Log($"{key} : References {imagesWithMissingID} image{(imagesWithMissingID > 1 ? "s" : "")} with missing ID.", LogLevel.Warn);

                        var textsWithMissingID = entry.Texts.Select(i => (!i.Disabled && (i.ID == null || i.ID == DisplayDataHelper.MISSING_ID_STR)) ? 1 : 0).Sum();
                        if (textsWithMissingID > 0)
                            SMonitor.Log($"{key} : References {textsWithMissingID} text{(textsWithMissingID > 1 ? "s" : "")} with missing ID.", LogLevel.Warn);

                        var dividersWithMissingID = entry.Dividers.Select(i => (!i.Disabled && (i.ID == null || i.ID == DisplayDataHelper.MISSING_ID_STR)) ? 1 : 0).Sum();
                        if (dividersWithMissingID > 0)
                            SMonitor.Log($"{key} : References {dividersWithMissingID} divider{(dividersWithMissingID > 1 ? "s" : "")} with missing ID.", LogLevel.Warn);

                        hasModsWithMissingID = hasModsWithMissingID || (imagesWithMissingID + textsWithMissingID + dividersWithMissingID > 0);
                    }

                    if (hasModsWithMissingID)
                        SMonitor.Log($"Please make sure to include a unique ID on each image, text and divider entries for better support.", LogLevel.Warn);
        }

                    DialogueBoxInterface.InvalidateCache();
                });
            }
        }

        private void GameLoop_GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            // register mod
            configMenu.Register(
                mod: ModManifest,
                reset: () => Config = new ModConfig(),
                save: () => Helper.WriteConfig(Config)
            );

            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => "Mod Enabled",
                getValue: () => Config.EnableMod,
                setValue: value => Config.EnableMod = value
            );
        }

        private void GameLoop_UpdateTicked_PostCP(object sender, UpdateTickedEventArgs e)
        {
            if (validationDelay-- == 0)
            {
                // Load our data to trigger validation
                SHelper.GameContent.Load<Dictionary<string, DialogueDisplayData>>(dictAssetName);
                SHelper.Events.GameLoop.UpdateTicked -= GameLoop_UpdateTicked_PostCP;
            }
        }
    }
}