using StardewValley.BellsAndWhistles;

namespace DialogueDisplayFramework.Data
{
    public class TextData : BaseData, ITextData
    {
        public string ID { get; set; }
        public string Color { get; set; }
        public string Text { get; set; }
        public bool Junimo { get; set; }
        public bool Scroll { get; set; }
        public string PlaceholderText { get; set; }
        public int ScrollType { get; set; }
        public SpriteText.ScrollTextAlignment Alignment { get; set; }
        public bool Centered { get; set; } // Deprecated

        public TextData()
        {
            ID = DisplayDataUtils.MISSING_ID_STR;
            ScrollType = -1;
            Alignment = SpriteText.ScrollTextAlignment.Center;
        }
    }
}
