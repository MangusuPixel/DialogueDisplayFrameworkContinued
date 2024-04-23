using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System.Collections.Generic;

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
        private static string listDelimiter = ", ";
        private static Dictionary<string, Texture2D> imageDict = new Dictionary<string, Texture2D>();

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = Helper.ReadConfig<ModConfig>();

            context = this;

            SMonitor = Monitor;
            SHelper = helper;

            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;

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
                e.LoadFrom(() => new Dictionary<string, DialogueDisplayData>(), AssetLoadPriority.Exclusive);
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
                    var keyGroups = new Dictionary<string, string>();

                    //SMonitor.Log($"Loaded {data.Count} data entries");

                    if (!data.ContainsKey(defaultKey))
                        data[defaultKey] = new DialogueDisplayData() { disabled = true };

                    imageDict.Clear();
                    foreach (var (key, entry) in data)
                    {
                        if (key.Contains(listDelimiter))
                        {
                            keyGroups.Add(entry.packName, key);
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
                    }

                    foreach (var (packName, originalKey) in keyGroups)
                    {
                        var refData = data[originalKey];

                        foreach (var key in originalKey.Split(listDelimiter))
                        {
                            if (!data.ContainsKey(key))
                            {
                                data[key] = refData;
                            }
                            else
                            {
                                SMonitor.Log(string.Format("Duplicate NPC '{0}' will be ignored in pack '{1}'.", key, packName), LogLevel.Warn);
                            }
                        }

                        data.Remove(originalKey);
                    }
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

    }
}