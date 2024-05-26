namespace DialogueDisplayFramework.Data
{
    public interface IPortraitData : IBaseData
    {
        public string TexturePath { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public bool TileSheet { get; set; }
    }
}
