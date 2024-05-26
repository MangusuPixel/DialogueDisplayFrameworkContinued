﻿namespace DialogueDisplayFramework.Data
{
    public interface IImageData : IBaseData
    {
        public string ID { get; set; }
        public string TexturePath { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
    }
}
