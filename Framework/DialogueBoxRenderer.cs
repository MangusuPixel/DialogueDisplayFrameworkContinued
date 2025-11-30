using DialogueDisplayFramework.Data;
using DialogueDisplayFramework.Api;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Menus;
using System;

namespace DialogueDisplayFramework.Framework
{
    public class DialogueBoxRenderer
    {
        public static void DrawDialogueBox(SpriteBatch b, DialogueDisplay display)
        {
            var data = display.Data;

            ApiConsumerManager.RaiseRenderingDialogueBox(b, display, data);

            if (data.Dividers != null)
                foreach (var divider in data.Dividers)
                    DrawDivider(b, display, divider);

            if (data.Images != null)
                foreach (var image in data.Images)
                    DrawImage(b, display, image);

            DrawPortrait(b, display, data.Portrait);
            DrawName(b, display, data.Name);

            if (data.Texts != null)
                foreach (var textData in data.Texts)
                    DrawText(b, display, textData);

            if (Game1.player.friendshipData.TryGetValue(display.Box.characterDialogue.speaker.Name, out Friendship friendship))
            {
                DrawHearts(b, display, data.Hearts, friendship);
                DrawGifts(b, display, data.Gifts, friendship);
                DrawJewel(b, display, data.Jewel, friendship);
            }

            DrawDialogueString(b, display, data.Dialogue);

            if (display.Box.dialogueIcon != null)
                DrawButton(b, display, data.Button);

            ApiConsumerManager.RaiseRenderedDialogueBox(b, display, data.GetAdapter());
        }

        public static void DrawDivider(SpriteBatch b, DialogueDisplay display, DividerData data)
        {
            ApiConsumerManager.RaiseRenderingDivider(b, display, data);

            if (data.Disabled.GetValueOrDefault() == false)
            {
                var pos = GetDataVector(display.Box, data);
                var color = Utility.StringToColor(data.Color) ?? Color.White;

                if (data.Horizontal == true && data.Width.HasValue)
                {
                    Texture2D texture = (data.Color is null) ? Game1.menuTexture : Game1.uncoloredMenuTexture;
                    b.Draw(
                        texture,
                        new Rectangle((int)pos.X, (int)pos.Y, data.Width.Value, data.Height ?? 64),
                        new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 6, -1, -1)),
                        color
                    );
                }
                else if (data.Height.HasValue)
                {
                    var divHeight = (data.Height < 1) ? display.Box.height + 4 : data.Height.Value;

                    b.Draw(
                        Game1.mouseCursors,
                        new Rectangle((int)pos.X, (int)pos.Y, 36, divHeight),
                        new Rectangle?(new Rectangle(278, 324, 9, 1)),
                        color
                    );

                    if (data.TopConnector == true)
                    {
                        b.Draw(
                            Game1.mouseCursors,
                            new Vector2(pos.X, pos.Y - 20),
                            new Rectangle?(new Rectangle(278, 313, 10, 7)),
                            color,
                            rotation: 0f,
                            origin: Vector2.Zero,
                            data.Scale ?? 4,
                            SpriteEffects.None,
                            data.LayerDepth ?? 0.88f
                        );
                    }

                    if (data.BottomConnector == true)
                    {
                        b.Draw(
                            Game1.mouseCursors,
                            new Vector2(pos.X, pos.Y + divHeight - 4),
                            new Rectangle?(new Rectangle(278, 328, 10, 8)),
                            color,
                            rotation: 0f,
                            origin: Vector2.Zero,
                            data.Scale ?? 4,
                            SpriteEffects.None,
                            data.LayerDepth ?? 0.88f
                        );
                    }
                }
            }

            ApiConsumerManager.RaiseRenderedDivider(b, display, data);
        }

        public static void DrawImage(SpriteBatch b, DialogueDisplay display, ImageData data)
        {
            ApiConsumerManager.RaiseRenderingImage(b, display, data);

            if (data.Disabled.GetValueOrDefault() == false &&
                data.W > 0 && data.H > 0 && data.TryGetTexture(out var texture)
            )
            {
                b.Draw(
                    texture,
                    GetDataVector(display.Box, data),
                    new Rectangle(data.X ?? 0, data.Y ?? 0, data.W.Value, data.H.Value),
                    Color.White * (data.Alpha ?? 1),
                    rotation: 0f,
                    origin: Vector2.Zero,
                    data.Scale ?? 4,
                    SpriteEffects.None,
                    data.LayerDepth ?? 0.88f
                );
            }

            ApiConsumerManager.RaiseRenderedImage(b, display, data);
        }

        public static void DrawPortrait(SpriteBatch b, DialogueDisplay display, PortraitData data)
        {
            ApiConsumerManager.RaiseRenderingPortrait(b, display, data);

            if (data.Disabled.GetValueOrDefault() == false && data.W > 0 && data.H > 0)
            {
                Texture2D portraitTexture = display.Box.characterDialogue.overridePortrait ??
                    display.Box.characterDialogue.speaker.Portrait;
                Rectangle portraitSource;

                if (data.TexturePath != null && !ModEntry.ImageDict[data.TexturePath].IsDisposed)
                {
                    portraitTexture = ModEntry.ImageDict[data.TexturePath];
                }

                if (data.X >= 0 && data.Y >= 0)
                {
                    portraitSource = new Rectangle(data.X ?? 0, data.Y ?? 0, data.W.Value, data.H.Value);
                }
                else
                {
                    portraitSource = Game1.getSourceRectForStandardTileSheet(portraitTexture,
                        display.Box.characterDialogue.getPortraitIndex(), data.W.Value, data.H.Value);
                }

                if (!portraitTexture.Bounds.Contains(portraitSource))
                {
                    portraitSource.X = 0;
                    portraitSource.Y = 0;
                }

                var offset = new Vector2(display.ShouldPortraitShake() ? Game1.random.Next(-1, 2) : 0, 0);

                if (!portraitTexture.IsDisposed)
                {
                    b.Draw(
                        portraitTexture,
                        GetDataVector(display.Box, data) + offset,
                        new Rectangle?(portraitSource),
                        Color.White * (data.Alpha ?? 1),
                        rotation: 0f,
                        origin: Vector2.Zero,
                        data.Scale ?? 4,
                        SpriteEffects.None,
                        data.LayerDepth ?? 0.88f
                    );
                }
            }

            ApiConsumerManager.RaiseRenderedPortrait(b, display, data);
        }

        public static void DrawName(SpriteBatch b, DialogueDisplay display, TextData data)
        {
            if (data.Disabled.GetValueOrDefault() == false)
            {
                data.Text = display.Box.characterDialogue.speaker.getName();
                data.MarkAsSpeakerDisplayName();
                DrawText(b, display, data);
            }
        }

        public static void DrawText(SpriteBatch b, DialogueDisplay display, TextData data)
        {
            ApiConsumerManager.RaiseRenderingText(b, display, data);

            if (data.Disabled.GetValueOrDefault() == false && data.Width > 0)
            {
                var pos = GetDataVector(display.Box, data);

                if (data.Centered == true || data.Alignment == SpriteText.ScrollTextAlignment.Center)
                    pos.X -= SpriteText.getWidthOfString(data.PlaceholderText ?? data.Text) / 2;
                else if (data.Alignment == SpriteText.ScrollTextAlignment.Right)
                    pos.X -= SpriteText.getWidthOfString(data.PlaceholderText ?? data.Text);

                SpriteText.drawString(b,
                    data.Text,
                    (int)pos.X,
                    (int)pos.Y,
                    width: data.Width.Value,
                    alpha: data.Alpha ?? default,
                    layerDepth: data.LayerDepth ?? 0.88f,
                    junimoText: data.Junimo.GetValueOrDefault(),
                    drawBGScroll: data.ScrollType ?? -1,
                    placeHolderScrollWidthText: data.PlaceholderText ?? "",
                    color: Utility.StringToColor(data.Color),
                    scroll_text_alignment: data.Alignment.GetValueOrDefault()
                );
            }

            ApiConsumerManager.RaiseRenderedText(b, display, data);
        }

        public static void DrawHearts(SpriteBatch b, DialogueDisplay display, HeartsData data, Friendship friendship)
        {
            ApiConsumerManager.RaiseRenderingHearts(b, display, data);

            if (data.Disabled.GetValueOrDefault() == false && data.HeartsPerRow > 0)
            {
                var speaker = display.Box.characterDialogue.speaker;
                int friendshipLevel = Game1.player.getFriendshipLevelForNPC(speaker.Name);
                bool isRomanceLocked = speaker.datable.Value && !friendship.IsDating() && !friendship.IsMarried() &&
                    !ModEntry.SHelper.ModRegistry.IsLoaded("Cherry.PlatonicRelationships");

                int heartLevel = friendshipLevel / 250;
                int maxHearts = Utility.GetMaximumHeartsForCharacter(speaker);
                int heartsToDisplay = data.ShowEmptyHearts.GetValueOrDefault() ? 
                    maxHearts + (isRomanceLocked ? 2 : 0) :
                    heartLevel + (data.ShowPartialhearts.GetValueOrDefault() && heartLevel < maxHearts ? 1 : 0);

                var heartsWidth = 7;
                var heartsHeight = 6;
                var heartsXSpacing = 1;
                var heartsYSpacing = 1;

                var pos = GetDataVector(display.Box, data);

                if (data.Centered == true)
                {
                    pos.X -= (Math.Min(heartsToDisplay,
                        data.HeartsPerRow.Value) * (heartsWidth + heartsXSpacing) - heartsXSpacing) / 2 * (data.Scale ?? 4);
                }

                var drawingPos = new Vector2(pos.X, pos.Y);
                var drawingColIndex = 0;
                var drawingRowIndex = 0;
                var remainingFriendshipLevel = friendshipLevel;

                for (int i = 0; i < heartsToDisplay; i++)
                {
                    int xSource = ((i < heartLevel) || (isRomanceLocked && i >= 8)) ? 211 : 218;
                    var color = (isRomanceLocked && i >= 8) ? (Color.Black * 0.35f) : Color.White;

                    b.Draw(
                        Game1.mouseCursors,
                        drawingPos,
                        new Rectangle(xSource, 428, 7, 6),
                        color,
                        rotation: 0,
                        origin: Vector2.Zero,
                        data.Scale ?? 4,
                        SpriteEffects.None,
                        data.LayerDepth ?? 0.88f
                    );

                    if (data.ShowPartialhearts.GetValueOrDefault() && remainingFriendshipLevel < 250)
                    {
                        float heartFullness = remainingFriendshipLevel / 250f;
                        b.Draw(
                            Game1.mouseCursors,
                            drawingPos,
                            new Rectangle(211, 428, (int)(7 * heartFullness), 6),
                            Color.White,
                            rotation: 0,
                            origin: Vector2.Zero,
                            data.Scale ?? 4,
                            SpriteEffects.None,
                            data.LayerDepth ?? 0.88f
                        );
                    }

                    if (++drawingColIndex < data.HeartsPerRow)
                    {
                        drawingPos.X += (heartsWidth + heartsXSpacing) * (data.Scale ?? 4);
                    }
                    else
                    {
                        drawingColIndex = 0;
                        drawingRowIndex++;

                        drawingPos.X = pos.X;
                        drawingPos.Y += (heartsHeight + heartsYSpacing) * (data.Scale ?? 4);

                        if (data.Centered == true && i > (heartsToDisplay - data.HeartsPerRow.Value) && 
                            heartsToDisplay % data.HeartsPerRow.Value > 0)
                        {
                            drawingPos.X += ((data.HeartsPerRow.Value - heartsToDisplay % data.HeartsPerRow.Value) *
                                (heartsWidth + heartsXSpacing)) / 2 * (data.Scale ?? 4);
                        }
                    }

                    remainingFriendshipLevel -= 250;
                }
            }

            ApiConsumerManager.RaiseRenderedHearts(b, display, data);
        }

        public static void DrawGifts(SpriteBatch b, DialogueDisplay display, GiftsData data, Friendship friendship)
        {
            ApiConsumerManager.RaiseRenderingGifts(b, display, data);

            var speaker = display.Box.characterDialogue.speaker;

            if (data.Disabled.GetValueOrDefault() == false && !friendship.IsMarried() && speaker is not Child)
            {
                var pos = GetDataVector(display.Box, data);

                var giftsThisWeek = Game1.player.friendshipData[speaker.Name].GiftsThisWeek;
                var iconRealScale = data.IconScale.Value * (data.Scale ?? 4);

                var iconRect = new Rectangle(166, 174, 14, 12);
                var emptyBoxRect = new Rectangle(227, 425, 9, 9);
                var checkmarkRect = new Rectangle(236, 425, 9, 9);

                if (data.IconScale > 0)
                {
                    var offset = Vector2.Zero;
                    if (data.Inline == true)
                        offset.Y = (iconRect.Height - iconRect.Height * data.IconScale.Value) / 2f * (data.Scale ?? 4);
                    else
                        offset.X = (checkmarkRect.Width * 2 - iconRect.Width * data.IconScale.Value - 1) / 2f * (data.Scale ?? 4);

                    Utility.drawWithShadow(b, Game1.mouseCursors2, pos + offset, iconRect,
                        Color.White, 0f, Vector2.Zero, iconRealScale, false, 0.88f, 0, -1, 0.2f);
                    pos += (data.Inline.GetValueOrDefault() ? new Vector2(iconRect.Width, 0) : new Vector2(0, iconRect.Height)) * iconRealScale;
                }

                // TODO: add padding option?
                pos += new Vector2(data.Inline == true ? 1 : 0, 2) * (data.Scale ?? 4);

                for (var i = 1; i >= 0; i--) // First checkmark is placed to the right
                {
                    b.Draw(
                        Game1.mouseCursors,
                        pos,
                        giftsThisWeek > i ? checkmarkRect : emptyBoxRect,
                        Color.White,
                        rotation: 0,
                        origin: Vector2.Zero,
                        data.Scale ?? 4,
                        SpriteEffects.None,
                        data.LayerDepth ?? 0.88f
                    );
                    pos.X += (checkmarkRect.Width - 1) * (data.Scale ?? 4);
                }
            }

            ApiConsumerManager.RaiseRenderedGifts(b, display, data);
        }

        public static void DrawJewel(SpriteBatch b, DialogueDisplay display, BaseData data, Friendship friendship)
        {
            ApiConsumerManager.RaiseRenderingJewel(b, display, data);

            if (ShouldDrawFriendshipJewel(display.Box) && data.Disabled.GetValueOrDefault() == false)
            {
                var pos = GetDataVector(display.Box, data);
                var friendshipHeartLevel = friendship.Points / 250;
                Rectangle sourceRect;

                if (friendshipHeartLevel >= 10)
                {
                    sourceRect = new Rectangle(269, 495, 11, 11);
                }
                else
                {
                    var animationFrame = (int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 1000.0 / 250.0);
                    sourceRect = new Rectangle(140 + animationFrame * 11, 532 + friendshipHeartLevel / 2 * 11, 11, 11);
                }

                b.Draw(
                    Game1.mouseCursors,
                    pos,
                    sourceRect,
                    Color.White * (data.Alpha ?? 1),
                    rotation: 0,
                    origin: Vector2.Zero,
                    data.Scale ?? 4,
                    SpriteEffects.None,
                    data.LayerDepth ?? 0.88f
                );

                display.Box.friendshipJewel = new Rectangle((int)pos.X, (int)pos.Y, (int)(11 * data.Scale), (int)(11 * data.Scale));
            } else
            {
                display.Box.friendshipJewel = Rectangle.Empty;
            }

            ApiConsumerManager.RaiseRenderedJewel(b, display, data);
        }

        public static void DrawDialogueString(SpriteBatch b, DialogueDisplay display, DialogueStringData data)
        {
            if (data == null) return;

            ApiConsumerManager.RaiseRenderingDialogueString(b, display, data);
            DialogueDisplayPatcher.SetDialogueStringBypass(false);

            var dialoguePos = GetDataVector(display.Box, data);
            var width = (data.Width >= 0 ? data.Width.Value : display.Box.width - 8) + ModEntry.Config.DialogueWidthOffset;

            SpriteText.drawString(b,
                display.Box.getCurrentString(),
                (int)dialoguePos.X,
                (int)dialoguePos.Y,
                characterPosition: display.Box.characterIndexInDialogue,
                width,
                alpha: data.Alpha ?? 1,
                layerDepth: data.LayerDepth ?? 0.88f,
                color: Utility.StringToColor(data.Color),
                scroll_text_alignment: data.Alignment.GetValueOrDefault()
            );

            DialogueDisplayPatcher.SetDialogueStringBypass(true);
            ApiConsumerManager.RaiseRenderedDialogueString(b, display, data);
        }

        public static void DrawButton(SpriteBatch b, DialogueDisplay display, BaseData data)
        {
            ApiConsumerManager.RaiseRenderingButton(b, display, data);

            if (data.Disabled.GetValueOrDefault() == false)
            {
                display.Box.dialogueIcon.position = GetDataVector(display.Box, data);
            }

            ApiConsumerManager.RaiseRenderedButton(b, display, data);
        }
        public static Vector2 GetDataVector(DialogueBox box, BaseData data)
        {
            return new Vector2(
                box.x + (data.Right == true ? box.width : 0) + data.XOffset.GetValueOrDefault(),
                box.y + (data.Bottom == true ? box.height : 0) + data.YOffset.GetValueOrDefault()
            );
        }

        // Extracted from StardewValley.Menus.DialogueBox.shouldDrawFriendshipJewel v1.6.15
        // Support for SMAPI for Android
        public static bool ShouldDrawFriendshipJewel(DialogueBox dlg)
        {
            return (dlg.width >= 642 && !Game1.eventUp && !dlg.isQuestion && dlg.isPortraitBox() &&
                !dlg.friendshipJewel.Equals(Rectangle.Empty) &&
                dlg.characterDialogue?.speaker != null &&
                Game1.player.friendshipData.ContainsKey(dlg.characterDialogue.speaker.Name) &&
                dlg.characterDialogue.speaker.Name != "Henchman"
            );
        }
    }
}
