using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Object = StardewValley.Object;

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
        private static Dictionary<string, DialogueDisplayData> dataDict = new Dictionary<string, DialogueDisplayData>();
        private static Dictionary<string, Texture2D> imageDict = new Dictionary<string, Texture2D>();
        private static List<string> loadedPacks = new List<string>();

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = Helper.ReadConfig<ModConfig>();

            context = this;

            SMonitor = Monitor;
            SHelper = helper;

            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            
            helper.Events.Content.AssetRequested += Content_AssetRequested;
            
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

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            LoadData();
        }

        private static void LoadData()
        {
            //SMonitor.Log("Loading Data");

            loadedPacks.Clear();
            dataDict.Clear();
            var rawDataDict = SHelper.GameContent.Load<Dictionary<string, DialogueDisplayData>>(dictPath);
            //SMonitor.Log($"Loaded {dataDict.Count} data entries");
            if (!rawDataDict.ContainsKey(defaultKey))
                dataDict[defaultKey] = new DialogueDisplayData() { disabled = true };

            imageDict.Clear();
            foreach(var dataEntry in rawDataDict)
            {
                var packName = dataEntry.Value.packName;
                var portraitData = dataEntry.Value.portrait;

                if (packName != null && !loadedPacks.Contains(packName))
                {
                    loadedPacks.Add(packName);
                }
                foreach (var image in dataEntry.Value.images)
                {
                    if(!imageDict.ContainsKey(image.texturePath))
                        imageDict[image.texturePath] = Game1.content.Load<Texture2D>(image.texturePath);
                }
                if (portraitData?.texturePath != null && !imageDict.ContainsKey(portraitData.texturePath))
                {
                    imageDict[portraitData.texturePath] = Game1.content.Load<Texture2D>(portraitData.texturePath);
                }
                foreach (var id in dataEntry.Key.Split(listDelimiter))
                {
                    if (!dataDict.ContainsKey(id))
                    {
                        dataDict[id] = dataEntry.Value;
                    } else
                    {
                        SMonitor.Log(string.Format("Duplicate NPC '{0}' will be ignored in pack '{1}'.", id, packName), LogLevel.Warn);
                    }
                }
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