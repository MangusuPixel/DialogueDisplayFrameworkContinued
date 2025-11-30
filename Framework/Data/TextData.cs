using StardewValley.BellsAndWhistles;
using System;

namespace DialogueDisplayFramework.Data
{
    public class TextData : BaseData, ITextData, IMergeableEntry<TextData>
    {
        private bool _isSpeakerDisplayName = false;

        public string ID { get; set; }
        public string Color { get; set; }
        public string Text { get; set; }
        public bool? Junimo { get; set; }
        public bool? Scroll { get; set; }
        public string PlaceholderText { get; set; }
        public int? ScrollType { get; set; }
        public SpriteText.ScrollTextAlignment? Alignment { get; set; }
        public bool? Centered { get; set; } // Deprecated

        public bool IsSpeakerDisplayName()
        {
            return _isSpeakerDisplayName;
        }

        internal void MarkAsSpeakerDisplayName()
        {
            _isSpeakerDisplayName = true;
        }

        public void MergeFrom(TextData other)
        {
            if (other == null || other == this)
                return;

            base.MergeFrom(other);
            Color ??= other.Color;
            Text ??= other.Text;
            Junimo ??= other.Junimo;
            Scroll ??= other.Scroll;
            PlaceholderText ??= other.PlaceholderText;
            ScrollType ??= other.ScrollType;
            Alignment ??= other.Alignment;
            Centered ??= other.Centered;
        }
    }
}
