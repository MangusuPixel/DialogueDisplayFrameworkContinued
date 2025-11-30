using StardewValley.BellsAndWhistles;

namespace DialogueDisplayFramework.Data
{
    public class DialogueStringData : BaseData, IDialogueStringData
    {
        public string Color { get; set; }
        public SpriteText.ScrollTextAlignment? Alignment { get; set; }

        public void MergeFrom(DialogueStringData other)
        {
            if (other == null || other == this)
                return;

            base.MergeFrom(other);
            Color ??= other.Color;
            Alignment ??= other.Alignment;
        }
    }
}
