using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogueDisplayFramework
{
    /// <summary>The mod entry point.</summary>
    public partial class ModEntry : Mod
    {
        public static IMonitor SMonitor;
        public static IModHelper SHelper;
        public static ModConfig Config;
        public static ModEntry context;

        private static string dictPath = "aedenthorn.DialogueDisplayFramework/dictionary";
        private static string defaultKey = "default";
        private static Dictionary<string, Texture2D> imageDict = new Dictionary<string, Texture2D>();
        private static bool dirtyDialogueData = false;

        private static int validationDelay = 5;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = Helper.ReadConfig<ModConfig>();

            context = this;

            SMonitor = Monitor;
            SHelper = helper;

            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
            helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked_PostCP;

            helper.Events.Content.AssetRequested += Content_AssetRequested;
            helper.Events.Content.AssetRequested += Content_AssetRequested_Post; // After CP edits

            var harmony = new Harmony(ModManifest.UniqueID);
            harmony.PatchAll();
        }

        private void Content_AssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (!Config.EnableMod)
                return;

            if (e.NameWithoutLocale.IsEquivalentTo(dictPath))
            {
                e.LoadFrom(() => new Dictionary<string, DialogueDisplayData>
                {
                    { defaultKey, DialogueDisplayData.DefaultValues }
                }, AssetLoadPriority.Exclusive);
            }
        }

        [EventPriority(EventPriority.Low)]
        private void Content_AssetRequested_Post(object sender, AssetRequestedEventArgs e)
        {
            if (!Config.EnableMod)
                return;

            if (e.NameWithoutLocale.IsEquivalentTo(dictPath))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, DialogueDisplayData>().Data;
                    var hasModsWithMissingID = false;

                    imageDict.Clear();

                    foreach (var (key, entry) in data)
                    {
                        if (entry.disabled)
                            continue;

                        // Validate copy references

                        if (entry.copyFrom != null)
                        {
                            var target = entry;
                            var traceStack = new List<DialogueDisplayData>() { target };

                            while (data.TryGetValue(target.copyFrom, out target))
                            {
                                traceStack.Add(target);

                                if (target.copyFrom == key)
                                {
                                    SMonitor.Log($"{String.Join(" > ", traceStack.Select(d => d.copyFrom))} > {entry.copyFrom} : Cyclic reference detected. Disabling.", LogLevel.Error);

                                    foreach (var k in traceStack)
                                        k.disabled = true;

                                    break;
                                }
                            }

                            if (!data.ContainsKey(entry.copyFrom))
                            {
                                SMonitor.Log($"{key} : CopyFrom key points to a non-existant entry: {entry.copyFrom}", LogLevel.Warn);
                            }
                        }

                        foreach (var image in entry.images)
                        {
                            if (!imageDict.ContainsKey(image.texturePath))
                                imageDict[image.texturePath] = Game1.content.Load<Texture2D>(image.texturePath);
                        }

                        if (entry.portrait?.texturePath != null && !imageDict.ContainsKey(entry.portrait.texturePath))
                        {
                            imageDict[entry.portrait.texturePath] = Game1.content.Load<Texture2D>(entry.portrait.texturePath);
                        }

                        var imagesWithMissingID = entry.images.Select(i => (!i.disabled && (i.ID == null || i.ID == DialogueDisplayData.MISSING_ID_STR)) ? 1 : 0).Sum();
                        if (imagesWithMissingID > 0)
                            SMonitor.Log($"{key} : References {imagesWithMissingID} divider{(imagesWithMissingID > 1 ? "s" : "")} with missing ID.", LogLevel.Warn);

                        var textsWithMissingID = entry.texts.Select(i => (!i.disabled && (i.ID == null || i.ID == DialogueDisplayData.MISSING_ID_STR)) ? 1 : 0).Sum();
                        if (textsWithMissingID > 0)
                            SMonitor.Log($"{key} : References {textsWithMissingID} divider{(textsWithMissingID > 1 ? "s" : "")} with missing ID.", LogLevel.Warn);

                        var dividersWithMissingID = entry.dividers.Select(i => (!i.disabled && (i.ID == null || i.ID == DialogueDisplayData.MISSING_ID_STR)) ? 1 : 0).Sum();
                        if (dividersWithMissingID > 0)
                            SMonitor.Log($"{key} : References {dividersWithMissingID} divider{(dividersWithMissingID > 1 ? "s" : "")} with missing ID.", LogLevel.Warn);

                        hasModsWithMissingID = hasModsWithMissingID || (imagesWithMissingID + textsWithMissingID + dividersWithMissingID > 0);
                    }

                    if (hasModsWithMissingID)
                        SMonitor.Log($"Please make sure to include a unique ID on each image, text and divider entries for better support.", LogLevel.Warn);
                });
            }

            dirtyDialogueData = true;
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
                SHelper.GameContent.Load<Dictionary<string, DialogueDisplayData>>(dictPath);
                SHelper.Events.GameLoop.UpdateTicked -= GameLoop_UpdateTicked_PostCP;
            }
        }
    }
}