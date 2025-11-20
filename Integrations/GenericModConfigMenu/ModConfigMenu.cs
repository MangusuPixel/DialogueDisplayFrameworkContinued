using StardewModdingAPI;

namespace DialogueDisplayFramework.Integrations.GenericModConfigMenu
{
    internal class ModConfigMenu
    {
        internal static readonly string ModID = "spacechase0.GenericModConfigMenu";
        internal static IModHelper Helper => ModEntry.SHelper;
        internal static IManifest ModManifest => ModEntry.SModManifest;
        internal static ModConfig Config => ModEntry.Config;

        internal static bool IsInstalled() => Helper.ModRegistry.IsLoaded(ModID);

        internal static void Register()
        {
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>(ModID);
            if (configMenu is null)
                return;

            configMenu.Register(
                mod: ModManifest,
                reset: Config.Reset,
                save: () => Helper.WriteConfig(Config)
            );

            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => "Allow Legacy Data",
                tooltip: () => "If enabled, will attempt to migrate data from the legacy dialogue display framework mod.",
                getValue: () => Config.UseLegacyData,
                setValue: value => Config.UseLegacyData = value
            );

            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => "Mod Enabled",
                getValue: () => Config.EnableMod,
                setValue: value => Config.EnableMod = value
            );

            configMenu.AddNumberOption(
                mod: ModManifest,
                name: () => "Dialogue Width Offset",
                tooltip: () => "Size offset to the dialogue box's width. Negative input shrinks size.\nDon't forget to adjust the x offset.",
                getValue: () => Config.DialogueWidthOffset,
                setValue: (value) => Config.DialogueWidthOffset = value
            );

            configMenu.AddNumberOption(
                mod: ModManifest,
                name: () => "Dialogue Height Offset",
                tooltip: () => "Size offset to the dialogue box's height. Negative input shrinks size.\nDon't forget to adjust the y offset.",
                getValue: () => Config.DialogueHeightOffset,
                setValue: (value) => Config.DialogueHeightOffset = value
            );

            configMenu.AddNumberOption(
                mod: ModManifest,
                name: () => "Dialogue X Offset",
                tooltip: () => "Position offset to the dialogue box's x position. Negative input moves the box to the left.",
                getValue: () => Config.DialogueXOffset,
                setValue: (value) => Config.DialogueXOffset = value
            );

            configMenu.AddNumberOption(
                mod: ModManifest,
                name: () => "Dialogue Y Offset",
                tooltip: () => "Position offset to the dialogue box's y position. Negative input moves the box up.",
                getValue: () => Config.DialogueYOffset,
                setValue: (value) => Config.DialogueYOffset = value
            );
        }
    }
}
