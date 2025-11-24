using DialogueDisplayFramework.Data;
using StardewModdingAPI;
using System.Collections.Generic;
using System.Linq;
using LegacyDisplayData = DialogueDisplayFramework.Legacy.Data.DialogueDisplayData;

namespace DialogueDisplayFramework.Framework
{
    internal class DataMigration
    {
        internal static List<DialogueDisplayData> MigrateFromLegacy(Dictionary<string, LegacyDisplayData> legacyData)
        {
            var migratedData = new List<DialogueDisplayData>();

            ModEntry.SMonitor.Log($"Found {legacyData.Count} outdated patches. Attempting to automatically convert them but some visual bugs are expected.", LogLevel.Warn);

            foreach(var (key, oldEntry) in legacyData)
            {
                if (oldEntry.Disabled)
                    continue;

                var newEntry = new DialogueDisplayData();
                TransferLegacyData(newEntry, oldEntry, legacyData);

                if (key != ModEntry.DefaultKey)
                {
                    var separatorIndex = key.IndexOf("_");
                    var speakerName = separatorIndex > 0 ? key[..(separatorIndex)] : key;
                    var metaInfo = separatorIndex > 0 ? key[(separatorIndex + 1)..] : "";

                    newEntry.DisplayCondition.Speaker = speakerName;

                    if (metaInfo.ToLower() == "beach")
                    {
                        newEntry.DisplayCondition.IsIslandAttire = true;
                    }
                    else if (metaInfo != "")
                    {
                        newEntry.DisplayCondition.AppearanceId = metaInfo;
                    }
                }

                migratedData.Add(newEntry);
            }

            return migratedData;
        }

        public static void TransferLegacyData(DialogueDisplayData target, LegacyDisplayData source, Dictionary<string, LegacyDisplayData> dataDict)
        {
            target.XOffset ??= source.XOffset;
            target.YOffset ??= source.YOffset;
            target.Width ??= source.Width;
            target.Height ??= source.Height;
            target.Dialogue ??= source.Dialogue;
            target.Portrait ??= source.Portrait;
            target.Name ??= source.Name;
            target.Jewel ??= source.Jewel;
            target.Button ??= source.Button;
            target.Gifts ??= source.Gifts;
            target.Images = (target.Images is null || source.Images is null) ? target.Images ?? source.Images : source.Images.Concat(target.Images).ToList();
            target.Texts = (target.Texts is null || source.Texts is null) ? target.Texts ?? source.Texts : source.Texts.Concat(target.Texts).ToList();
            target.Dividers = (target.Dividers is null || source.Dividers is null) ? target.Dividers ?? source.Dividers : source.Dividers.Concat(target.Dividers).ToList();

            if (!(source.CopyFrom is null or "") && dataDict.TryGetValue(source.CopyFrom, out var parentData) && !parentData.Disabled)
            {
                TransferLegacyData(target, parentData, dataDict);
            }
        }
    }
}
