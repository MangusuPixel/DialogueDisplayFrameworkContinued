using StardewValley.BellsAndWhistles;

namespace DialogueDisplayFramework.Data
{
    public interface ITextData : IBaseData
    {
        public string ID { get; set; }
        public string Color { get; set; }
        public string Text { get; set; }
        public bool Junimo { get; set; }
        public bool Scroll { get; set; }
        public string PlaceholderText { get; set; }
        public int ScrollType { get; set; }
        public SpriteText.ScrollTextAlignment Alignment { get; set; }
    }
}
