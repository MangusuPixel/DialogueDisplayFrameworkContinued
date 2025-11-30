namespace DialogueDisplayFramework.Data
{
    public class HeartsData : BaseData, IHeartsData
    {
        public int? HeartsPerRow { get; set; }
        public bool? ShowEmptyHearts { get; set; }
        public bool? ShowPartialhearts { get; set; }
        public bool? Centered { get; set; }

        public void MergeFrom(HeartsData other)
        {
            if (other == null || other == this)
                return;

            base.MergeFrom(other);
            HeartsPerRow ??= other.HeartsPerRow;
            ShowEmptyHearts ??= other.ShowEmptyHearts;
            ShowPartialhearts ??= other.ShowPartialhearts;
            Centered ??= other.Centered;
        }
    }
}
