namespace DialogueDisplayFramework.Data
{
    public class BaseData : IBaseData
    {
        public int? XOffset { get; set; }
        public int? YOffset { get; set; }
        public bool? Right { get; set; }
        public bool? Bottom { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public float? Alpha { get; set; }
        public float? Scale { get; set; }
        public float? LayerDepth { get; set; }
        public bool? Disabled { get; set; }

        public void MergeFrom(BaseData other)
        {
            if (other == null || other == this)
                return;

            XOffset ??= other.XOffset;
            YOffset ??= other.YOffset;
            Right ??= other.Right;
            Bottom ??= other.Bottom;
            Width ??= other.Width;
            Height ??= other.Height;
            Alpha ??= other.Alpha;
            Scale ??= other.Scale;
            LayerDepth ??= other.LayerDepth;
            Disabled ??= other.Disabled;
        }
    }
}
