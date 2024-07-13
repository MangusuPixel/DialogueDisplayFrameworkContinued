using System.Collections.Generic;

namespace DialogueDisplayFramework.Data
{
    public class DialogueDisplayData : IDialogueDisplayData
    {
        public string CopyFrom { get; set; }
        public string PackName { get; set; }
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
        public bool Disabled { get; set; }

        public DialogueDisplayData()
        {
            Images = new();
            Texts = new();
            Dividers = new();
        }
    }
}