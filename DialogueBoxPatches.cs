using DialogueDisplayFramework.Api;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;

namespace DialogueDisplayFramework
{
    public partial class ModEntry
    {
        internal class DialogueBoxPatches
        {
            public static void Dialogue_Postfix(DialogueBox __instance, Dialogue dialogue)
            {
                if (!Config.EnableMod || dialogue?.speaker == null)
                    return;

                try
                {
                    DialogueBoxInterface.InvalidateCache();

                    DialogueDisplayApi.Instance.DisplayData = data;
                    var data = DialogueBoxInterface.GetCharacterDisplay(__instance.characterDialogue.speaker);

                    if (data == null)
                        return;

                    __instance.x += data.XOffset ?? 0;
                    __instance.y += data.YOffset ?? 0;
                    if (data.Width > 0)
                        __instance.width = (int)data.Width;
                    if (data.Height > 0)
                        __instance.height = (int)data.Height;

                    DialogueBoxInterface.shouldPortraitShake = SHelper.Reflection.GetMethod(__instance, "shouldPortraitShake");
                }
                catch (Exception ex)
                {
                    SMonitor.Log($"Failed in {nameof(Dialogue_Postfix)}:\n{ex}", LogLevel.Error);
                    return;
                }
            }

            public static void GameWindowSizeChanged_Postfix(DialogueBox __instance)
            {
                if (!Config.EnableMod || __instance.characterDialogue?.speaker is null)
                    return;

                try
                {
                    var data = DialogueBoxInterface.GetCharacterDisplay(__instance.characterDialogue.speaker);
                    if (data == null)
                        return;

                    __instance.x += data.XOffset ?? 0;
                    __instance.y += data.YOffset ?? 0;
                    if (data.Width > 0)
                        __instance.width = (int)data.Width;
                    if (data.Height > 0)
                        __instance.height = (int)data.Height;
                }
                catch (Exception ex)
                {
                    SMonitor.Log($"Failed in {nameof(GameWindowSizeChanged_Postfix)}:\n{ex}", LogLevel.Error);
                    return;
                }
            }

            public static bool DrawPortrait_Prefix(DialogueBox __instance, SpriteBatch b)
            {
                if (!Config.EnableMod)
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

                    var data = DialogueDisplayApi.Instance.DisplayData;

                    DialogueBoxMethods.DrawDialogueBox(b, __instance, data);
                
                    return false;
                }
                catch (Exception ex)
                {
                    SMonitor.Log($"Failed in {nameof(DrawPortrait_Prefix)}:\n{ex}", LogLevel.Error);
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

            public static void Draw_Postfix(DialogueBox __instance, SpriteBatch b)
            {
                if (!Config.EnableMod)
                    return;

                DialogueBoxInterface.preventGetCurrentString = false;
            }
        }
    }
}