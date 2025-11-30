using DialogueDisplayFramework.Data;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;

namespace DialogueDisplayFramework.Framework
{
    internal class DialogueBoxPatches
    {
        public static DialogueDisplayData ActiveData { get; set; }

        private static ModConfig Config { get; set; }
        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        public static void Apply(Harmony harmony, ModConfig config, IMonitor monitor, IModHelper helper)
        {
            Config = config;
            Monitor = monitor;
            Helper = helper;

            harmony.Patch(
                original: AccessTools.Constructor(typeof(DialogueBox), new Type[] { typeof(Dialogue) }),
                postfix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(Dialogue_Postfix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.drawPortrait)),
                prefix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(DrawPortrait_Prefix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.getCurrentString)),
                prefix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(GetCurrentString_Prefix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.draw), new Type[] { typeof(SpriteBatch) }),
                prefix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(Draw_Prefix)),
                postfix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(Draw_Postfix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.update), new Type[] { typeof(GameTime) }),
                prefix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(Update_Prefix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.gameWindowSizeChanged)),
                postfix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(GameWindowSizeChanged_Postfix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.closeDialogue)),
                postfix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(CloseDialogue_Postfix))
            );

            if (Constants.TargetPlatform == GamePlatform.Android)
            {
                harmony.Patch(
                    original: AccessTools.Method(typeof(DialogueBox), "checkDialogue"),
                    postfix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(CheckDialogue_Postfix))
                );
            }

            harmony.Patch(
                original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.drawBox)),
                prefix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(DrawBox_Prefix))
            );
        }

        public static void Dialogue_Postfix(DialogueBox __instance, Dialogue dialogue)
        {
            if (!Config.EnableMod)
                return;

            try
            {
                DialogueDisplayPatcher.MarkDisplayPositionDirty();
                DialogueDisplayPatcher.SetupNewDisplay(__instance);
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(Dialogue_Postfix)}:\n{ex}", LogLevel.Error);
                return;
            }
        }

        public static bool DrawPortrait_Prefix(DialogueBox __instance, SpriteBatch b)
        {
            if (!Config.EnableMod || DialogueDisplayPatcher.CurrentDisplay is null)
                return true;

            try
            {
                DialogueDisplayPatcher.DrawDialogueDisplay(b);
                return false;
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(DrawPortrait_Prefix)}:\n{ex}", LogLevel.Error);
            }

            DialogueDisplayPatcher.SetDialogueStringBypass(false);

            return true;
        }

        public static bool GetCurrentString_Prefix(DialogueBox __instance, ref string __result)
        {
            try
            {
                if (Config.EnableMod && DialogueDisplayPatcher.GetDialogueStringBypass())
                {
                    __result = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(GetCurrentString_Prefix)}:\n{ex}", LogLevel.Error);
            }

            return true;
        }

        public static void Draw_Prefix(DialogueBox __instance, SpriteBatch b)
        {
            try
            {
                if (Config.EnableMod)
                {
                    DialogueDisplayPatcher.SetDialogueStringBypass(false);
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(Draw_Prefix)}:\n{ex}", LogLevel.Error);
            }
        }

        public static void Draw_Postfix(DialogueBox __instance, SpriteBatch b)
        {
            try
            {
                if (Config.EnableMod)
                {
                    DialogueDisplayPatcher.SetDialogueStringBypass(false);
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(Draw_Postfix)}:\n{ex}", LogLevel.Error);
            }
        }

        public static void Update_Prefix(DialogueBox __instance, GameTime time)
        {
            try
            {
                if (Config.EnableMod)
                {
                    DialogueDisplayPatcher.SetDialogueStringBypass(false);
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(Update_Prefix)}:\n{ex}", LogLevel.Error);
            }
        }

        public static void CloseDialogue_Postfix(DialogueBox __instance)
        {
            try
            {
                if (Config.EnableMod)
                {
                    DialogueDisplayPatcher.ClearCurrentDisplay();
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(CloseDialogue_Postfix)}:\n{ex}", LogLevel.Error);
                return;
            }
        }

        public static void GameWindowSizeChanged_Postfix(DialogueBox __instance)
        {
            try
            {
                if (Config.EnableMod && __instance.characterDialogue?.speaker is not null)
                {
                    // force update position on the same frame
                    DialogueDisplayPatcher.MarkDisplayPositionDirty();
                    DialogueDisplayPatcher.UpdateDisplayPositionAndSize();
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(GameWindowSizeChanged_Postfix)}:\n{ex}", LogLevel.Error);
            }
        }

        // Android-only!!
        public static void CheckDialogue_Postfix(DialogueBox __instance, Dialogue d)
        {
            try
            {
                // Android version sets box dimensions on left click so we need to re-apply configs
                if (Config.EnableMod && __instance.characterDialogue?.speaker is not null)
                {
                    // force update position on the same frame
                    DialogueDisplayPatcher.MarkDisplayPositionDirty();
                    DialogueDisplayPatcher.UpdateDisplayPositionAndSize();
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(CloseDialogue_Postfix)}:\n{ex}", LogLevel.Error);
                return;
            }
        }

        public static void DrawBox_Prefix(DialogueBox __instance, SpriteBatch b, int xPos, int yPos, int boxWidth, int boxHeight)
        {
            if (!Config.EnableMod && __instance.characterDialogue?.speaker != null)
                return;

            try
            {
                DialogueDisplayPatcher.UpdateDisplayPositionAndSize();
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(DrawBox_Prefix)}:\n{ex}", LogLevel.Error);
                return;
            }
        }
    }
}