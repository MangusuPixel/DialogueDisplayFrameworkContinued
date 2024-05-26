using System;
using System.Linq;
using System.Collections.Generic;
using StardewValley;
using StardewModdingAPI;
using DialogueDisplayFramework.Data;

namespace DialogueDisplayFramework
{
    public class DialogueBoxInterface
    {
        public static bool dirtyDialogueData = true;

        public static bool preventGetCurrentString;
        private static DialogueDisplayData cachedDialogueData;

        // Reflection
        public static IReflectedMethod shouldPortraitShake;

        // Get the correct data based on context
        public static DialogueDisplayData GetDialogueDisplayData(Dialogue characterDialogue)
        {
            // Cached object is reset when a new dialogue box is opened or the asset cache is invalidated
            if (!dirtyDialogueData)
                return cachedDialogueData;

            DialogueDisplayData displayData = null;

            var dataDict = ModEntry.SHelper.GameContent.Load<Dictionary<string, DialogueDisplayData>>(ModEntry.dictAssetName);
            NPC speaker = characterDialogue.speaker;

            // Location-specific attire key, for legacy support
            var location = speaker.currentLocation;
            if (location != null && location.TryGetMapProperty("UniquePortrait", out string uniquePortraitsProperty) && ArgUtility.SplitBySpace(uniquePortraitsProperty).Contains(speaker.Name))
                dataDict.TryGetValue(speaker.Name + "_" + location.Name, out displayData);

            // KeyCharacter appearance key
            if ((displayData == null || displayData.Disabled) && speaker.LastAppearanceId != null)
                dataDict.TryGetValue(speaker.Name + "_" + speaker.LastAppearanceId, out displayData);

            // Beach attire key
            if ((displayData == null || displayData.Disabled) && ModEntry.SHelper.Reflection.GetField<bool>(speaker, "isWearingIslandAttire").GetValue())
                dataDict.TryGetValue(speaker.Name + "_Beach", out displayData);

            // Regular character key
            if (displayData == null || displayData.Disabled)
                dataDict.TryGetValue(speaker.Name, out displayData);

            // Default key
            if (displayData == null || displayData.Disabled)
                dataDict.TryGetValue(ModEntry.defaultKey, out displayData);

            // Load templates
            var copyFromKey = displayData.CopyFrom;

            while (copyFromKey != null)
            {
                if (dataDict.TryGetValue(copyFromKey, out var copyFromData))
                {
                    displayData = DisplayDataHelper.MergeEntries(displayData, copyFromData);
                }

                copyFromKey = copyFromData?.CopyFrom;
            }

            cachedDialogueData = displayData;
            dirtyDialogueData = false;

            return displayData;
        }
    }
}
