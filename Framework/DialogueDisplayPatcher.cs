using DialogueDisplayFramework.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using System.Linq;

namespace DialogueDisplayFramework.Framework
{
    internal class DialogueDisplayPatcher
    {
        private static readonly ModConfig _config = ModEntry.Config;

        private static DialogueDisplay _currentDisplay;

        private static bool _displayPositionDirty;
        private static Vector2? _currentDisplayPosition;

        public static DialogueDisplay CurrentDisplay => _currentDisplay;

        public static void SetupNewDisplay(DialogueBox dialogueBox)
        {
            var display = new DialogueDisplay(dialogueBox);
            var patchDataPath = ModEntry.DataAssetName;
            var patchData = ModEntry.SHelper.GameContent.Load<List<DialogueDisplayData>>(patchDataPath);
            var speaker = dialogueBox.characterDialogue.speaker;

            foreach (var patch in Enumerable.Reverse(patchData))
            {
                if (patch.DisplayCondition is var conditions && (conditions.Disabled ||
                    (conditions.IsAndroid.HasValue && conditions.IsAndroid != (Constants.TargetPlatform == GamePlatform.Android)) ||
                    (conditions.BoxWidthEquals.HasValue && conditions.BoxWidthEquals != dialogueBox.width) ||
                    (conditions.BoxWidthOver.HasValue && conditions.BoxWidthOver <= dialogueBox.width) ||
                    (conditions.BoxWidthUnder.HasValue && conditions.BoxWidthUnder >= dialogueBox.width) ||
                    (conditions.CanSocialize.HasValue && conditions.CanSocialize != speaker.CanSocialize) ||
                    (conditions.CanBeRomanced.HasValue && conditions.CanBeRomanced != speaker.datable.Value) ||
                    (conditions.CanReceiveGifts.HasValue && conditions.CanReceiveGifts != speaker.CanReceiveGifts()) ||
                    (conditions.IsEventActor.HasValue && conditions.IsEventActor != speaker.EventActor) ||
                    (conditions.Gender.HasValue && conditions.Gender != speaker.Gender) ||
                    (conditions.Age.HasValue && conditions.Age != speaker.Age) ||
                    (conditions.IsBirthdayToday.HasValue && conditions.IsBirthdayToday != (speaker.Birthday_Day == Game1.dayOfMonth && speaker.Birthday_Season == Game1.currentSeason)) ||
                    (conditions.IsDatingPlayer.HasValue && conditions.IsDatingPlayer != display.GetSpeakerFriendship()?.IsDating()) ||
                    (conditions.GiftReceivedToday.HasValue && conditions.GiftReceivedToday != display.GetSpeakerFriendship()?.GiftsToday > 0) ||
                    (conditions.GiftsReceivedThisWeek.HasValue && conditions.GiftsReceivedThisWeek != display.GetSpeakerFriendship()?.GiftsThisWeek) ||
                    (conditions.FriendshipPointsEquals.HasValue && conditions.FriendshipPointsEquals != display.GetSpeakerFriendship()?.Points) ||
                    (conditions.FriendshipPointsOver.HasValue && conditions.FriendshipPointsOver <= display.GetSpeakerFriendship()?.Points) ||
                    (conditions.FriendshipPointsUnder.HasValue && conditions.FriendshipPointsUnder >= display.GetSpeakerFriendship()?.Points) ||
                    (!string.IsNullOrEmpty(conditions.Speaker) && !conditions.Speaker.ToLower().Contains(speaker.Name.ToLower())) ||
                    (!string.IsNullOrEmpty(conditions.AppearanceId) && conditions.AppearanceId.ToLower() != speaker.LastAppearanceId.ToLower()) ||
                    (!string.IsNullOrEmpty(conditions.Location) && conditions.Location.ToLower() != speaker.currentLocation.NameOrUniqueName.ToLower()) ||
                    (conditions.IsIslandAttire.HasValue && conditions.IsIslandAttire != display.GetIsWearingIslandAttire()) ||
                    (!string.IsNullOrEmpty(conditions.Query) && !GameStateQuery.CheckConditions(conditions.Query))
                ))
                    continue;

                display.Data.MergeFrom(patch);
            }

            DataHelpers.FillInDefaults(display.Data);

            _currentDisplay = display;
            MarkDisplayPositionDirty();
        }

        public static void DrawDialogueDisplay(SpriteBatch b)
        {
            DialogueBoxRenderer.DrawDialogueBox(b, _currentDisplay);
        }

        public static void MarkDisplayPositionDirty()
        {
            _displayPositionDirty = true;
        }

        public static void UpdateDisplayPositionAndSize()
        {
            if (!_displayPositionDirty || _currentDisplay == null)
                return;

            var dialogueBox = _currentDisplay.Box;

            if (dialogueBox.isPortraitBox() && !dialogueBox.isQuestion)
            {
                var boxPos = new Vector2(_config.DialogueXOffset, _config.DialogueYOffset);

                var data = _currentDisplay.Data;

                if (data != null)
                {
                    if (data.Width > 0)
                        dialogueBox.width = (int)data.Width;
                    if (data.Height > 0)
                        dialogueBox.height = (int)data.Height;

                    boxPos += new Vector2(data.XOffset ?? 0, data.YOffset ?? 0);
                }

                dialogueBox.width += _config.DialogueWidthOffset;
                dialogueBox.height += _config.DialogueHeightOffset;

                if (boxPos != _currentDisplayPosition)
                {
                    var corrections = boxPos - (_currentDisplayPosition ?? Vector2.Zero);

                    dialogueBox.x += (int)corrections.X;
                    dialogueBox.y += (int)corrections.Y;

                    _currentDisplayPosition = boxPos;
                }
            }
        }

        public static void SetDialogueStringBypass(bool shouldOverride)
        {
            if (_currentDisplay != null)
                _currentDisplay.PreventGetCurrentString = shouldOverride;
        }

        public static bool GetDialogueStringBypass()
        {
            return _currentDisplay?.PreventGetCurrentString ?? false;
        }

        public static void ClearCurrentDisplay()
        {
            _currentDisplay = null;
        }
    }
}
