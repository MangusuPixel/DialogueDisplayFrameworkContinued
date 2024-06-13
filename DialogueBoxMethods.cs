using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Menus;
using DialogueDisplayFramework.Data;
using DialogueDisplayFramework.Api;
using System.Xml.Linq;
using StardewValley.Locations;

namespace DialogueDisplayFramework
{
    public class DialogueBoxMethods
    {
        public static void DrawDialogueBox(SpriteBatch b, DialogueBox dialogueBox, DialogueDisplayData data)
        {
            DialogueDisplayApi.Instance.Events.RenderingDialogueBox.Raise(b, dialogueBox, data);

            if (data.Dividers != null)
                foreach (var divider in data.Dividers)
                    DrawDivider(b, dialogueBox, divider);

            if (data.Images != null)
                foreach (var image in data.Images)
                    DrawImage(b, dialogueBox, image);

            DrawPortrait(b, dialogueBox, data.Portrait);
            DrawName(b, dialogueBox, data.Name);

            if (data.Texts != null)
                foreach (var textData in data.Texts)
                    DrawText(b, dialogueBox, textData);

            if (Game1.player.friendshipData.TryGetValue(dialogueBox.characterDialogue.speaker.Name, out Friendship friendship))
            {
                DrawHearts(b, dialogueBox, data.Hearts, friendship);
                DrawGifts(b, dialogueBox, data.Gifts, friendship);
                DrawJewel(b, dialogueBox, data.Jewel, friendship);
            }

            var dialogue = (data.Dialogue != null && !data.Dialogue.Disabled) ? data.Dialogue : DisplayDataHelper.DefaultValues.Dialogue;
            DrawDialogueString(b, dialogueBox, dialogue);

            if (dialogueBox.dialogueIcon != null)
                DrawButton(b, dialogueBox, data.Button);

            DialogueDisplayApi.Instance.Events.RenderedDialogueBox.Raise(b, dialogueBox, data);
        }

        public static void DrawDivider(SpriteBatch b, DialogueBox dialogueBox, DividerData divider)
        {
            DialogueDisplayApi.Instance.Events.RenderingDivider.Raise(b, dialogueBox, divider);

            if (divider?.Disabled == false)
            {
                var pos = DialogueBoxDrawUtils.GetDataVector(dialogueBox, divider);
                var color = Utility.StringToColor(divider.Color) ?? Color.White;

                if (divider.Horizontal)
                {
                    Texture2D texture = (divider.Color is null) ? Game1.menuTexture : Game1.uncoloredMenuTexture;
                    b.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, divider.Width, 64), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 6, -1, -1)), color);
                }
                else
                {
                    var divHeight = (divider.Height < 1) ? dialogueBox.height + 4 : divider.Height;

                    b.Draw(Game1.mouseCursors, new Rectangle((int)pos.X, (int)pos.Y, 36, divHeight), new Rectangle?(new Rectangle(278, 324, 9, 1)), color);

                    if (divider.Connectors?.Top == true)
                        b.Draw(Game1.mouseCursors, new Vector2(pos.X, pos.Y - 20), new Rectangle?(new Rectangle(278, 313, 10, 7)), color, 0f, Vector2.Zero, divider.Scale, SpriteEffects.None, divider.LayerDepth);

                    if (divider.Connectors?.Bottom == true)
                        b.Draw(Game1.mouseCursors, new Vector2(pos.X, pos.Y + divHeight - 4), new Rectangle?(new Rectangle(278, 328, 10, 8)), color, 0f, Vector2.Zero, divider.Scale, SpriteEffects.None, divider.LayerDepth);
                }
            }

            DialogueDisplayApi.Instance.Events.RenderedDivider.Raise(b, dialogueBox, divider);
        }

        public static void DrawImage(SpriteBatch b, DialogueBox dialogueBox, ImageData image)
        {
            DialogueDisplayApi.Instance.Events.RenderingImage.Raise(b, dialogueBox, image);

            if (image?.Disabled == false)
            {
                var texture = ModEntry.ImageDict[image.TexturePath];
                var pos = DialogueBoxDrawUtils.GetDataVector(dialogueBox, image);

                b.Draw(texture, pos, new Rectangle(image.X, image.Y, image.W, image.H), Color.White * image.Alpha, 0, Vector2.Zero, image.Scale, SpriteEffects.None, image.LayerDepth);
            }

            DialogueDisplayApi.Instance.Events.RenderedImage.Raise(b, dialogueBox, image);
        }

        public static void DrawPortrait(SpriteBatch b, DialogueBox dialogueBox, PortraitData portrait)
        {
            DialogueDisplayApi.Instance.Events.RenderingPortrait.Raise(b, dialogueBox, portrait);

            if (portrait?.Disabled == false)
            {
                Texture2D portraitTexture = dialogueBox.characterDialogue.overridePortrait ?? dialogueBox.characterDialogue.speaker.Portrait;
                Rectangle portraitSource;

                if (portrait.TexturePath != null)
                {
                    portraitTexture = ModEntry.ImageDict[portrait.TexturePath];
                }

                if (portrait.X >= 0 && portrait.Y >= 0)
                {
                    portraitSource = new Rectangle(portrait.X, portrait.Y, portrait.W, portrait.H);
                }
                else
                {
                    portraitSource = Game1.getSourceRectForStandardTileSheet(portraitTexture, dialogueBox.characterDialogue.getPortraitIndex(), portrait.W, portrait.H);
                }

                if (!portraitTexture.Bounds.Contains(portraitSource))
                {
                    portraitSource.X = 0;
                    portraitSource.Y = 0;
                }

                var offset = new Vector2(DialogueBoxInterface.shouldPortraitShake.Invoke<bool>(dialogueBox.characterDialogue) ? Game1.random.Next(-1, 2) : 0, 0);

                b.Draw(portraitTexture, DialogueBoxDrawUtils.GetDataVector(dialogueBox, portrait) + offset, new Rectangle?(portraitSource), Color.White * portrait.Alpha, 0f, Vector2.Zero, portrait.Scale, SpriteEffects.None, portrait.LayerDepth);
            }

            DialogueDisplayApi.Instance.Events.RenderedPortrait.Raise(b, dialogueBox, portrait);
        }

        public static void DrawName(SpriteBatch b, DialogueBox dialogueBox, TextData name)
        {
            if (name?.Disabled == false)
            {
                name.Text = dialogueBox.characterDialogue.speaker.getName();
                name.ID = DisplayDataHelper.TEXT_NAME_ID;
                DrawText(b, dialogueBox, name);
            }
        }

        public static void DrawText(SpriteBatch b, DialogueBox dialogueBox, TextData data)
        {
            DialogueDisplayApi.Instance.Events.RenderingText.Raise(b, dialogueBox, data);

            if (data?.Disabled == false)
            {
                var pos = DialogueBoxDrawUtils.GetDataVector(dialogueBox, data);

                if (data.Centered || data.Alignment == SpriteText.ScrollTextAlignment.Center)
                    pos.X -= SpriteText.getWidthOfString(data.PlaceholderText ?? data.Text) / 2;
                else if (data.Alignment == SpriteText.ScrollTextAlignment.Right)
                    pos.X -= SpriteText.getWidthOfString(data.PlaceholderText ?? data.Text);

                SpriteText.drawString(b, data.Text, (int)pos.X, (int)pos.Y, 999999, data.Width, 999999, data.Alpha, data.LayerDepth, data.Junimo, data.ScrollType, data.PlaceholderText ?? "", Utility.StringToColor(data.Color), data.Alignment);
            }

            DialogueDisplayApi.Instance.Events.RenderedText.Raise(b, dialogueBox, data);
        }

        public static void DrawHearts(SpriteBatch b, DialogueBox dialogueBox, HeartsData hearts, Friendship friendship)
        {
            DialogueDisplayApi.Instance.Events.RenderingHearts.Raise(b, dialogueBox, hearts);

            if (hearts?.Disabled == false)
            {
                var speaker = dialogueBox.characterDialogue.speaker;
                int friendshipLevel = Game1.player.getFriendshipLevelForNPC(speaker.Name);
                bool isRomanceLocked = speaker.datable.Value && !friendship.IsDating() && !friendship.IsMarried();
                int heartLevel = friendshipLevel / 250;
                int maxHearts = Utility.GetMaximumHeartsForCharacter(speaker);
                int heartsToDisplay = hearts.ShowEmptyHearts ? maxHearts + (isRomanceLocked ? 2 : 0) : heartLevel + (hearts.ShowPartialhearts && heartLevel < maxHearts ? 1 : 0);

                var heartsWidth = 7;
                var heartsHeight = 6;
                var heartsXSpacing = 1;
                var heartsYSpacing = 1;

                var pos = DialogueBoxDrawUtils.GetDataVector(dialogueBox, hearts);

                if (hearts.Centered)
                {
                    pos.X -= (Math.Min(heartsToDisplay, hearts.HeartsPerRow) * (heartsWidth + heartsXSpacing) - heartsXSpacing) / 2 * hearts.Scale;
                }

                var drawingPos = new Vector2(pos.X, pos.Y);
                var drawingColIndex = 0;
                var drawingRowIndex = 0;
                var remainingFriendshipLevel = friendshipLevel;

                for (int i = 0; i < heartsToDisplay; i++)
                {
                    int xSource = ((i < heartLevel) || (isRomanceLocked && i >= 8)) ? 211 : 218;
                    var color = (isRomanceLocked && i >= 8) ? (Color.Black * 0.35f) : Color.White;

                    b.Draw(Game1.mouseCursors, drawingPos, new Rectangle(xSource, 428, 7, 6), color, 0, Vector2.Zero, hearts.Scale, SpriteEffects.None, hearts.LayerDepth);

                    if (hearts.ShowPartialhearts && remainingFriendshipLevel < 250)
                    {
                        float heartFullness = remainingFriendshipLevel / 250f;
                        b.Draw(Game1.mouseCursors, drawingPos, new Rectangle(211, 428, (int)(7 * heartFullness), 6), Color.White, 0, Vector2.Zero, hearts.Scale, SpriteEffects.None, hearts.LayerDepth);
                    }

                    if (++drawingColIndex < hearts.HeartsPerRow)
                    {
                        drawingPos.X += (heartsWidth + heartsXSpacing) * hearts.Scale;
                    }
                    else
                    {
                        drawingColIndex = 0;
                        drawingRowIndex++;

                        drawingPos.X = pos.X;
                        drawingPos.Y += (heartsHeight + heartsYSpacing) * hearts.Scale;

                        if (hearts.Centered && i > (heartsToDisplay - hearts.HeartsPerRow) && heartsToDisplay % hearts.HeartsPerRow > 0)
                        {
                            drawingPos.X += ((hearts.HeartsPerRow - heartsToDisplay % hearts.HeartsPerRow) * (heartsWidth + heartsXSpacing)) / 2 * hearts.Scale;
                        }
                    }

                    remainingFriendshipLevel -= 250;
                }
            }

            DialogueDisplayApi.Instance.Events.RenderedHearts.Raise(b, dialogueBox, hearts);
        }

        public static void DrawGifts(SpriteBatch b, DialogueBox dialogueBox, GiftsData gifts, Friendship friendship)
        {
            DialogueDisplayApi.Instance.Events.RenderingGifts.Raise(b, dialogueBox, gifts);

            var speaker = dialogueBox.characterDialogue.speaker;

            if (gifts?.Disabled == false && !friendship.IsMarried() && speaker is not Child)
            {
                var pos = DialogueBoxDrawUtils.GetDataVector(dialogueBox, gifts);
                Utility.drawWithShadow(b, Game1.mouseCursors2, pos + new Vector2(6, 0), new Rectangle(166, 174, 14, 12), Color.White, 0f, Vector2.Zero, 4f, false, 0.88f, 0, -1, 0.2f);
                b.Draw(Game1.mouseCursors, pos + (gifts.Inline ? new Vector2(64, 8) : new Vector2(0, 56)), new Rectangle?(new Rectangle(227 + ((Game1.player.friendshipData[speaker.Name].GiftsThisWeek >= 2) ? 9 : 0), 425, 9, 9)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.88f);
                b.Draw(Game1.mouseCursors, pos + (gifts.Inline ? new Vector2(96, 8) : new Vector2(32, 56)), new Rectangle?(new Rectangle(227 + (Game1.player.friendshipData[speaker.Name].GiftsThisWeek >= 1 ? 9 : 0), 425, 9, 9)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.88f);
            }

            DialogueDisplayApi.Instance.Events.RenderedGifts.Raise(b, dialogueBox, gifts);
        }

        public static void DrawJewel(SpriteBatch b, DialogueBox dialogueBox, BaseData jewel, Friendship friendship)
        {
            DialogueDisplayApi.Instance.Events.RenderingJewel.Raise(b, dialogueBox, jewel);

            if (dialogueBox.shouldDrawFriendshipJewel() && jewel?.Disabled == false)
            {
                var friendshipHeartLevel = friendship.Points / 250;
                Rectangle sourceRect;

                if (friendshipHeartLevel >= 10)
                    sourceRect = new Rectangle(269, 494, 11, 11);
                else
                {
                    var animationFrame = (int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 1000.0 / 250.0);
                    sourceRect = new Rectangle(140 + animationFrame * 11, 532 + friendshipHeartLevel / 2 * 11, 11, 11);
                }

                b.Draw(Game1.mouseCursors, DialogueBoxDrawUtils.GetDataVector(dialogueBox, jewel), sourceRect, Color.White * jewel.Alpha, 0f, Vector2.Zero, jewel.Scale, SpriteEffects.None, jewel.LayerDepth);
            }

            DialogueDisplayApi.Instance.Events.RenderedJewel.Raise(b, dialogueBox, jewel);
        }

        public static void DrawDialogueString(SpriteBatch b, DialogueBox dialogueBox, DialogueStringData dialogue)
        {
            DialogueDisplayApi.Instance.Events.RenderingDialogueString.Raise(b, dialogueBox, dialogue);

            var dialoguePos = DialogueBoxDrawUtils.GetDataVector(dialogueBox, dialogue);

            DialogueBoxInterface.preventGetCurrentString = false;

            SpriteText.drawString(b, dialogueBox.getCurrentString(), (int)dialoguePos.X, (int)dialoguePos.Y, dialogueBox.characterIndexInDialogue, dialogue.Width >= 0 ? dialogue.Width : dialogueBox.width - 8, 999999, dialogue.Alpha, dialogue.LayerDepth, false, -1, "", Utility.StringToColor(dialogue.Color), dialogue.Alignment);

            DialogueBoxInterface.preventGetCurrentString = true;

            DialogueDisplayApi.Instance.Events.RenderedDialogueString.Raise(b, dialogueBox, dialogue);
        }

        public static void DrawButton(SpriteBatch b, DialogueBox dialogueBox, BaseData button)
        {
            DialogueDisplayApi.Instance.Events.RenderingButton.Raise(b, dialogueBox, button);

            if (button?.Disabled == false)
            {
                dialogueBox.dialogueIcon.position = DialogueBoxDrawUtils.GetDataVector(dialogueBox, button);
            }

            DialogueDisplayApi.Instance.Events.RenderedButton.Raise(b, dialogueBox, button);
        }
    }
}
