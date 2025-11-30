using System.Collections.Generic;
using System.Linq;

namespace DialogueDisplayFramework.Data
{
    public class DialogueDisplayData
    {
        private readonly DialogueDisplayDataAdapter _adapter;

        public DialogueDisplayData()
        {
            _adapter = new(this);
        }

        public string Id { get; set; }
        public int? XOffset { get; set; }
        public int? YOffset { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public DialogueStringData Dialogue { get; set; }
        public PortraitData Portrait { get; set; }
        public TextData Name { get; set; }
        public BaseData Jewel { get; set; }
        public BaseData Button { get; set; }
        public GiftsData Gifts { get; set; }
        public HeartsData Hearts { get; set; }
        public List<ImageData> Images { get; set; }
        public List<TextData> Texts { get; set; }
        public List<DividerData> Dividers { get; set; }
        public DisplayConditionData DisplayCondition { get; set; } = new();

        /// <summary>
        /// Fetches the adapter that can be used in other assemblies which map to the <see cref="IDialogueDisplayApi"/> interface.
        /// </summary>
        /// <returns>An adapter that implements <see cref="IDialogueDisplayData"/></returns>
        public DialogueDisplayDataAdapter GetAdapter()
        {
            return _adapter;
        }

        public void MergeFrom(DialogueDisplayData other)
        {
            if (other == null || other == this)
                return;

            XOffset ??= other.XOffset;
            YOffset ??= other.YOffset;
            Width = other.Width;
            Height = other.Height;
            Dialogue ??= other.Dialogue;
            Portrait ??= other.Portrait;
            Name ??= other.Name;
            Jewel ??= other.Jewel;
            Button ??= other.Button;
            Gifts ??= other.Gifts;
            Hearts ??= other.Hearts;

            Dialogue?.MergeFrom(other.Dialogue);
            Portrait?.MergeFrom(other.Portrait);
            Name?.MergeFrom(other.Name);
            Jewel?.MergeFrom(other.Jewel);
            Button?.MergeFrom(other.Button);
            Gifts?.MergeFrom(other.Gifts);
            Hearts?.MergeFrom(other.Hearts);

            Images = MergeLists(Images, other.Images);
            Texts = MergeLists(Texts, other.Texts);
            Dividers = MergeLists(Dividers, other.Dividers);
        }

        private static List<T> MergeLists<T>(List<T> targetList, List<T> sourceList)
            where T: IMergeableEntry<T>
        {
            if (sourceList == null || sourceList.Count == 0)
                return targetList;

            if (targetList == null || targetList.Count == 0)
                return sourceList;

            foreach (var entry in sourceList)
            {
                var duplicatedEntry = targetList.FirstOrDefault(x => x.ID == entry.ID);
                if (duplicatedEntry == null)
                    targetList.Add(entry);
                else
                    duplicatedEntry.MergeFrom(entry);
            }

            return targetList;
        }
    }
}