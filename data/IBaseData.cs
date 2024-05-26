namespace DialogueDisplayFramework.Data
{
    public interface IBaseData
    {
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public bool Right { get; set; }
        public bool Bottom { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float Alpha { get; set; }
        public float Scale { get; set; }
        public float LayerDepth { get; set; }
        public bool Disabled { get; set; }
    }
}
