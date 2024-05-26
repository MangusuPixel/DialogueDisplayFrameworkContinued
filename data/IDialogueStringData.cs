using StardewValley.BellsAndWhistles;

namespace DialogueDisplayFramework.Data
{
    public interface IDialogueStringData : IBaseData
    {
        public string Color { get; set; }
        public SpriteText.ScrollTextAlignment Alignment { get; set; }
    }
}
