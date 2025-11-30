namespace DialogueDisplayFramework.Data
{
    public class GiftsData : BaseData, IGiftsData
    {
        public float? IconScale { get; set; }
        public bool? Inline { get; set; }

        public void MergeFrom(GiftsData other)
        {
            if (other == null || other == this)
                return;

            base.MergeFrom(other);
            IconScale ??= other.IconScale;
            Inline ??= other.Inline;
        }
    }
}
