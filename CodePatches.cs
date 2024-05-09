using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogueDisplayFramework
{
    public partial class ModEntry
    {
        private static bool preventGetCurrentString;
        private static DialogueDisplayData cachedDialogueData;
        //private static ProfileMenu npcSpriteMenu;

        // Reflection
        private static IReflectedMethod shouldPortraitShake;

        // Get the correct data based on context
        public static DialogueDisplayData GetDialogueDisplayData(Dialogue characterDialogue)
        {
            if (!dirtyDialogueData)
                return cachedDialogueData;

            var dataDict = SHelper.GameContent.Load<Dictionary<string, DialogueDisplayData>>(dictPath);
            DialogueDisplayData dataFound = null;

            NPC speaker = characterDialogue.speaker;
            var location = speaker.currentLocation;

            // Location-specific attire key, for legacy support
            if (location != null && location.TryGetMapProperty("UniquePortrait", out string uniquePortraitsProperty) && ArgUtility.SplitBySpace(uniquePortraitsProperty).Contains(speaker.Name))
                dataDict.TryGetValue(speaker.Name + "_" + location.Name, out dataFound);

            // KeyCharacter appearance key
            if ((dataFound == null || dataFound.disabled) && speaker.LastAppearanceId != null)
                dataDict.TryGetValue(speaker.Name + "_" + speaker.LastAppearanceId, out dataFound);

            // Beach attire key
            if ((dataFound == null || dataFound.disabled) && SHelper.Reflection.GetField<bool>(speaker, "isWearingIslandAttire").GetValue())
                dataDict.TryGetValue(speaker.Name + "_Beach", out dataFound);

            // Regular character key
            if (dataFound == null || dataFound.disabled)
                dataDict.TryGetValue(speaker.Name, out dataFound);

            // Default key
            if (dataFound == null || dataFound.disabled)
                dataDict.TryGetValue(defaultKey, out dataFound);

            cachedDialogueData = dataFound;
            dirtyDialogueData = false;

            return dataFound;
        }

        private static Vector2 GetDataVector(DialogueBox box, BaseData data)
        {
            return new Vector2(box.x + (data.right ? box.width : 0) + data.xOffset, box.y + (data.bottom ? box.height : 0) + data.yOffset);
        }

        private static void DrawTextComponent(SpriteBatch b, DialogueBox box, TextData data)
        {
            var pos = GetDataVector(box, data);

            if (data.centered || data.alignment == SpriteText.ScrollTextAlignment.Center)
                pos.X -= SpriteText.getWidthOfString(data.placeholderText ?? data.text) / 2;
            else if (data.alignment == SpriteText.ScrollTextAlignment.Right)
                pos.X -= SpriteText.getWidthOfString(data.placeholderText ?? data.text);

            SpriteText.drawString(b, data.text, (int)pos.X, (int)pos.Y, 999999, data.width, 999999, data.alpha, data.layerDepth, data.junimo, data.scrollType, data.placeholderText ?? "", Utility.StringToColor(data.color), data.alignment);
        }

        [HarmonyPatch(typeof(DialogueBox), new Type[] { typeof(Dialogue) })]
        [HarmonyPatch(MethodType.Constructor)]
        public class DialogueBox_Patch
        {
            public static void Postfix(DialogueBox __instance, Dialogue dialogue)
            {
                if (!Config.EnableMod || dialogue?.speaker is null)
                    return;
                /*
                try
                {
                    npcSpriteMenu = new ProfileMenu(dialogue.speaker);
                }
                catch
                {

                }
                */
                dirtyDialogueData = true;
                var data = GetDialogueDisplayData(__instance.characterDialogue);
                if (data == null)
                    return;

                __instance.x += data.xOffset;
                __instance.y += data.yOffset;
                if (data.width > 0)
                    __instance.width = data.width;
                if (data.height > 0)
                    __instance.height = data.height;

                shouldPortraitShake = SHelper.Reflection.GetMethod(__instance, "shouldPortraitShake");
            }
        }

        [HarmonyPatch(typeof(DialogueBox), nameof(DialogueBox.receiveLeftClick))]
        public class DialogueBox_receiveLeftClick_Patch
        {
            public static bool Prefix(DialogueBox __instance, int x, int y, bool playSound)
            {
                if (!Config.EnableMod || __instance.characterDialogue?.speaker is null)
                    return true;

                var data = GetDialogueDisplayData(__instance.characterDialogue);
                if (data == null)
                    return true;
                /*
                var sprite = data.sprite is null ? dataDict[defaultKey].sprite : data.sprite;
                if (sprite is not null && !sprite.disabled)
                {
                    if(new Rectangle(Utility.Vector2ToPoint(GetDataVector(__instance, sprite)), new Point(128, 192)).Contains(new Point(x, y)))
                    {
                        ProfileMenu menu = new ProfileMenu(__instance.characterDialogue.speaker);
                        Game1.activeClickableMenu = menu;
                        return false;
                    }
                }
                */
                return true;
            }
        }

        [HarmonyPatch(typeof(DialogueBox), nameof(DialogueBox.gameWindowSizeChanged))]
        public class DialogueBox_gameWindowSizeChanged_Patch
        {
            public static void Postfix(DialogueBox __instance)
            {
                if (!Config.EnableMod || __instance.characterDialogue?.speaker is null)
                    return;

                var data = GetDialogueDisplayData(__instance.characterDialogue);
                if (data == null)
                    return;

                __instance.x += data.xOffset;
                __instance.y += data.yOffset;
                if (data.width > 0)
                    __instance.width = data.width;
                if (data.height > 0)
                    __instance.height = data.height;
            }
        }

        [HarmonyPatch(typeof(DialogueBox), nameof(DialogueBox.drawPortrait))]
        public class DialogueBox_drawPortrait_Patch
        {
            public static bool Prefix(DialogueBox __instance, SpriteBatch b)
            {
                if (!Config.EnableMod)
                    return true;
                NPC speaker = __instance.characterDialogue.speaker;

                var defaultData = SHelper.GameContent.Load<Dictionary<string, DialogueDisplayData>>(dictPath)[defaultKey];
                var data = GetDialogueDisplayData(__instance.characterDialogue);
                if (defaultData == null && data == null)
                    return true;

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

                // Dividers

                var dividers = data.dividers is null ? defaultData.dividers : data.dividers;

                if (dividers != null)
                {
                    foreach (var divider in dividers)
                    {
                        var pos = GetDataVector(__instance, divider);
                        var color = Utility.StringToColor(divider.color) ?? Color.White;

                        if (divider.horizontal)
                        {
                            Texture2D texture = (divider.color is null) ? Game1.menuTexture : Game1.uncoloredMenuTexture;
                            b.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, divider.width, 64), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 6, -1, -1)), color);
                        }
                        else
                        {
                            var divHeight = (divider.height < 1) ? __instance.height + 4 : divider.height;

                            b.Draw(Game1.mouseCursors, new Rectangle((int)pos.X, (int)pos.Y, 36, divHeight), new Rectangle?(new Rectangle(278, 324, 9, 1)), color);

                            if (divider.connectors?.top == true)
                                b.Draw(Game1.mouseCursors, new Vector2(pos.X, pos.Y - 20), new Rectangle?(new Rectangle(278, 313, 10, 7)), color, 0f, Vector2.Zero, divider.scale, SpriteEffects.None, divider.layerDepth);

                            if (divider.connectors?.bottom == true)
                                b.Draw(Game1.mouseCursors, new Vector2(pos.X, pos.Y + divHeight - 4), new Rectangle?(new Rectangle(278, 328, 10, 8)), color, 0f, Vector2.Zero, divider.scale, SpriteEffects.None, divider.layerDepth);
                        }
                    }
                }

                // Images

                var images = data.images is null ? defaultData.images : data.images;

                if (images != null)
                {
                    foreach (var image in images)
                    {
                        b.Draw(imageDict[image.texturePath], GetDataVector(__instance, image), new Rectangle(image.x, image.y, image.w, image.h), Color.White * image.alpha, 0, Vector2.Zero, image.scale, SpriteEffects.None, image.layerDepth);
                    }
                }

                // NPC Portrait

                var portrait = data.portrait is null ? defaultData.portrait : data.portrait;

                if (portrait is not null && !portrait.disabled)
                {
                    Texture2D portraitTexture = __instance.characterDialogue.overridePortrait ?? speaker.Portrait;
                    Rectangle portraitSource;

                    if (portrait.texturePath != null)
                    {
                        portraitTexture = imageDict[portrait.texturePath];
                    }

                    if (portrait.x >= 0 && portrait.y >= 0)
                    {
                        portraitSource = new Rectangle(portrait.x, portrait.y, portrait.w, portrait.h);
                    }
                    else
                    {
                        portraitSource = Game1.getSourceRectForStandardTileSheet(portraitTexture, __instance.characterDialogue.getPortraitIndex(), portrait.w, portrait.h);
                    }

                    if (!portraitTexture.Bounds.Contains(portraitSource))
                    {
                        portraitSource.X = 0;
                        portraitSource.Y = 0;
                    }

                    var offset = new Vector2(shouldPortraitShake.Invoke<bool>(__instance.characterDialogue) ? Game1.random.Next(-1, 2) : 0, 0);

                    b.Draw(portraitTexture, GetDataVector(__instance, portrait) + offset, new Rectangle?(portraitSource), Color.White * portrait.alpha, 0f, Vector2.Zero, portrait.scale, SpriteEffects.None, portrait.layerDepth);
                }

                // Sprite
                /*
                var sprite = data.sprite is null ? dataDict[defaultKey].sprite : data.sprite;

                if (sprite is not null && !sprite.disabled && npcSpriteMenu is not null)
                {
                    var pos = GetDataVector(__instance, sprite);

                    if (sprite.background)
                    {
                        b.Draw((Game1.timeOfDay >= 1900) ? Game1.nightbg : Game1.daybg, pos, Color.White);
                    }

                    if (sprite.frame >= 0)
                    {
                        AccessTools.FieldRefAccess<ProfileMenu, AnimatedSprite>(npcSpriteMenu, "_animatedSprite").CurrentFrame = sprite.frame;
                    }
                    else
                    {
                        npcSpriteMenu.update(Game1.currentGameTime);

                    }
                    AccessTools.FieldRefAccess<ProfileMenu, AnimatedSprite>(npcSpriteMenu, "_animatedSprite").draw(b, pos + new Vector2(32, 32), sprite.layerDepth, 0, 0, Color.White, false, sprite.scale, 0, false);
                }
                */

                // NPC Name

                var npcName = data.name != null ? data.name : defaultData.name;
                if (npcName is not null && !npcName.disabled)
                {
                    npcName.text = speaker.getName();
                    DrawTextComponent(b, __instance, npcName);
                }

                // Texts

                var texts = data.texts is null ? defaultData.texts : data.texts;

                if (texts != null)
                {
                    foreach (var textData in texts)
                    {
                        DrawTextComponent(b, __instance, textData);
                    }
                }

                if (Game1.player.friendshipData.ContainsKey(speaker.Name))
                {
                    // Hearts

                    var hearts = data.hearts is null ? defaultData.hearts : data.hearts;
                    if (hearts is not null && !hearts.disabled)
                    {
                        int friendshipLevel = Game1.player.getFriendshipLevelForNPC(speaker.Name);
                        bool isRomanceLocked = speaker.datable.Value && Game1.player.friendshipData.TryGetValue(speaker.Name, out Friendship friendship) && !friendship.IsDating() && !friendship.IsMarried();
                        int heartLevel = friendshipLevel / 250;
                        int maxHearts = Utility.GetMaximumHeartsForCharacter(speaker);
                        int heartsToDisplay = hearts.showEmptyHearts ? maxHearts + (isRomanceLocked ? 2 : 0) : heartLevel + (hearts.showPartialhearts && heartLevel < maxHearts ? 1 : 0);

                        var heartsWidth = 7;
                        var heartsHeight = 6;
                        var heartsXSpacing = 1;
                        var heartsYSpacing = 1;

                        var pos = GetDataVector(__instance, hearts);

                        if (hearts.centered)
                        {
                            pos.X -= (Math.Min(heartsToDisplay, hearts.heartsPerRow) * (heartsWidth + heartsXSpacing) - heartsXSpacing) / 2 * hearts.scale;
                        }

                        var drawingPos = new Vector2(pos.X, pos.Y);
                        var drawingColIndex = 0;
                        var drawingRowIndex = 0;
                        var remainingFriendshipLevel = friendshipLevel;

                        for (int i = 0; i < heartsToDisplay; i++)
                        {
                            int xSource = ((i < heartLevel) || (isRomanceLocked && i >= 8)) ? 211 : 218;
                            var color = (isRomanceLocked && i >= 8) ? (Color.Black * 0.35f) : Color.White;

                            b.Draw(Game1.mouseCursors, drawingPos, new Rectangle(xSource, 428, 7, 6), color, 0, Vector2.Zero, hearts.scale, SpriteEffects.None, hearts.layerDepth);

                            if (hearts.showPartialhearts && remainingFriendshipLevel < 250)
                            {
                                float heartFullness = remainingFriendshipLevel / 250f;
                                b.Draw(Game1.mouseCursors, drawingPos, new Rectangle(211, 428, (int) (7 * heartFullness), 6), Color.White, 0, Vector2.Zero, hearts.scale, SpriteEffects.None, hearts.layerDepth);
                            }

                            if (++drawingColIndex < hearts.heartsPerRow)
                            {
                                drawingPos.X += (heartsWidth + heartsXSpacing) * hearts.scale;
                            }
                            else
                            {
                                drawingColIndex = 0;
                                drawingRowIndex++;

                                drawingPos.X = pos.X;
                                drawingPos.Y += (heartsHeight + heartsYSpacing) * hearts.scale;

                                if (hearts.centered && i > (heartsToDisplay - hearts.heartsPerRow) && heartsToDisplay % hearts.heartsPerRow > 0)
                                {
                                    drawingPos.X += ((hearts.heartsPerRow - heartsToDisplay % hearts.heartsPerRow) * (heartsWidth + heartsXSpacing)) / 2 * hearts.scale;
                                }
                            }

                            remainingFriendshipLevel -= 250;
                        }
                    }

                    // Gifts

                    var gifts = data.gifts is null ? defaultData.gifts : data.gifts;
                    if (gifts is not null && !gifts.disabled && !Game1.player.friendshipData[speaker.Name].IsMarried() && Game1.getCharacterFromName(speaker.Name) is not Child)
                    {
                        var pos = GetDataVector(__instance, gifts);
                        Utility.drawWithShadow(b, Game1.mouseCursors2, pos + new Vector2(6, 0), new Rectangle(166, 174, 14, 12), Color.White, 0f, Vector2.Zero, 4f, false, 0.88f, 0, -1, 0.2f);
                        b.Draw(Game1.mouseCursors, pos + (gifts.inline ? new Vector2(64, 8) : new Vector2(0, 56)), new Rectangle?(new Rectangle(227 + ((Game1.player.friendshipData[speaker.Name].GiftsThisWeek >= 2) ? 9 : 0), 425, 9, 9)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.88f);
                        b.Draw(Game1.mouseCursors, pos + (gifts.inline ? new Vector2(96, 8) : new Vector2(32, 56)), new Rectangle?(new Rectangle(227 + (Game1.player.friendshipData[speaker.Name].GiftsThisWeek >= 1 ? 9 : 0), 425, 9, 9)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.88f);
                    }

                    // Jewel

                    if (__instance.shouldDrawFriendshipJewel())
                    {
                        var jewel = data.jewel is null ? defaultData.jewel : data.jewel;
                        if (jewel != null && !jewel.disabled)
                        {
                            var friendshipHeartLevel = Game1.player.getFriendshipHeartLevelForNPC(speaker.Name);
                            Rectangle sourceRect;

                            if (friendshipHeartLevel >= 10)
                                sourceRect = new Rectangle(269, 494, 11, 11);
                            else
                            {
                                var animationFrame = (int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 1000.0 / 250.0);
                                sourceRect = new Rectangle(140 + animationFrame * 11, 532 + friendshipHeartLevel / 2 * 11, 11, 11);
                            }

                            b.Draw(Game1.mouseCursors, GetDataVector(__instance, jewel), sourceRect, Color.White * jewel.alpha, 0f, Vector2.Zero, jewel.scale, SpriteEffects.None, jewel.layerDepth);
                        }
                    }
                }

                // Dialogue String

                var dialogue = data.dialogue is null ? defaultData.dialogue : data.dialogue;
                var dialoguePos = GetDataVector(__instance, dialogue);
                preventGetCurrentString = false;
                SpriteText.drawString(b, __instance.getCurrentString(), (int)dialoguePos.X, (int)dialoguePos.Y, __instance.characterIndexInDialogue, dialogue.width >= 0 ? dialogue.width : __instance.width - 8, 999999, dialogue.alpha, dialogue.layerDepth, false, -1, "", Utility.StringToColor(dialogue.color), dialogue.alignment);

                // Close Icon

                if (__instance.dialogueIcon != null)
                {
                    var button = data.button is null ? defaultData.button : data.button;

                    if (button != null && !button.disabled)
                        __instance.dialogueIcon.position = GetDataVector(__instance, button);
                }
                preventGetCurrentString = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(DialogueBox), nameof(DialogueBox.getCurrentString))]
        public class DialogueBox_getCurrentString_Patch
        {
            public static bool Prefix(DialogueBox __instance, ref string __result)
            {
                if (!Config.EnableMod || !preventGetCurrentString)
                    return true;
                __result = "";
                return false;
            }
        }

        [HarmonyPatch(typeof(DialogueBox), nameof(DialogueBox.draw))]
        public class DialogueBox_draw_Patch
        {
            public static void Postfix(DialogueBox __instance, SpriteBatch b)
            {
                if (!Config.EnableMod)
                    return;

                preventGetCurrentString = false;
            }
        }
    }
}