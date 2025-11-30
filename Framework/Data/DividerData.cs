namespace DialogueDisplayFramework.Data
{
    public class DividerData : BaseData, IDividerData, IMergeableEntry<DividerData>
    {
        public string ID { get; set; }
        public bool? Horizontal { get; set; }
        public bool? Small { get; set; }
        public string Color { get; set; }
        public bool? TopConnector { get; set; } = true;
        public bool? BottomConnector { get; set; } = true;

        public void MergeFrom(DividerData other)
        {
            if (other == null || other == this)
                return;

            base.MergeFrom(other);
            Horizontal ??= other.Horizontal;
            Small ??= other.Small;
            Color ??= other.Color;
            TopConnector ??= other.TopConnector;
            BottomConnector ??= other.BottomConnector;
        }
    }
}
