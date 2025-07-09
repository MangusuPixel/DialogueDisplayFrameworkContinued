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
        public static DialogueDisplayData activeData;

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
                original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.drawBox)),
                prefix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(DrawBox_Prefix))
            );
        }

        public static void Dialogue_Postfix(DialogueBox __instance, Dialogue dialogue)
        {
            if (!Config.EnableMod || dialogue?.speaker is null)
                return;

            try
            {
                DialogueBoxInterface.InvalidateCache();
                DialogueBoxInterface.appliedBoxSize = false;

                // cache reflection calls
                DialogueBoxInterface.shouldPortraitShake = Helper.Reflection.GetMethod(__instance, "shouldPortraitShake");
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(Dialogue_Postfix)}:\n{ex}", LogLevel.Error);
                return;
            }
        }

        public static bool DrawPortrait_Prefix(DialogueBox __instance, SpriteBatch b)
        {
            if (!Config.EnableMod || activeData is null)
                return true;

            try
            {
                NPC speaker = __instance.characterDialogue.speaker;

                if (!Game1.IsMasterGame && !speaker.EventActor)
                {
                    var currentLocation = speaker.currentLocation;
                    if (currentLocation == null || !currentLocation.IsActiveLocation())
                    {
                        NPC actualSpeaker = Game1.getCharacterFromName(speaker.Name, true, false);
                        if (actualSpeaker != null && actualSpeaker.currentLocation.IsActiveLocation())
                            speaker = actualSpeaker;
                    }
                }

                DialogueBoxRenderer.DrawDialogueBox(b, __instance, activeData);

                return false;
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(DrawPortrait_Prefix)}:\n{ex}", LogLevel.Error);
            }

            DialogueBoxInterface.preventGetCurrentString = false;
            return true;
        }

        public static bool GetCurrentString_Prefix(DialogueBox __instance, ref string __result)
        {
            if (!Config.EnableMod || !DialogueBoxInterface.preventGetCurrentString)
                return true;
            __result = "";
            return false;
        }

        public static void Draw_Prefix(DialogueBox __instance, SpriteBatch b)
        {
            if (!Config.EnableMod)
                return;

            DialogueBoxInterface.preventGetCurrentString = false;
        }

        public static void Draw_Postfix(DialogueBox __instance, SpriteBatch b)
        {
            if (!Config.EnableMod)
                return;

            DialogueBoxInterface.preventGetCurrentString = false;
        }

        public static void Update_Prefix(DialogueBox __instance, GameTime time)
        {
            if (!Config.EnableMod)
                return;

            DialogueBoxInterface.preventGetCurrentString = false;
        }

        public static void GameWindowSizeChanged_Postfix(DialogueBox __instance)
        {
            if (!Config.EnableMod || __instance.characterDialogue?.speaker is null)
                return;

            try
            {
                UpdateDialogueBoxSize(__instance);
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(GameWindowSizeChanged_Postfix)}:\n{ex}", LogLevel.Error);
                return;
            }
        }

        public static void DrawBox_Prefix(DialogueBox __instance, SpriteBatch b, int xPos, int yPos, int boxWidth, int boxHeight)
        {
            if (!Config.EnableMod || __instance.characterDialogue?.speaker is null || DialogueBoxInterface.appliedBoxSize)
                return;

            try
            {
                UpdateDialogueBoxSize(__instance);
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(DrawBox_Prefix)}:\n{ex}", LogLevel.Error);
                return;
            }
        }

        public static void UpdateDialogueBoxSize(DialogueBox dialogueBox)
        {
            if (!dialogueBox.isPortraitBox() || dialogueBox.isQuestion)
                return;

            activeData = DialogueBoxInterface.GetCharacterDisplay(dialogueBox.characterDialogue.speaker);

            if (activeData == null)
                return;

            dialogueBox.x += activeData.XOffset ?? 0;
            dialogueBox.y += activeData.YOffset ?? 0;
            if (activeData.Width > 0)
                dialogueBox.width = (int)activeData.Width;
            if (activeData.Height > 0)
                dialogueBox.height = (int)activeData.Height;

            DialogueBoxInterface.appliedBoxSize = true;
        }
    }
}