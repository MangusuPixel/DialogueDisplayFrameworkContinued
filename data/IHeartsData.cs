namespace DialogueDisplayFramework.Data
{
    public interface IHeartsData : IBaseData
    {
        public int HeartsPerRow { get; set; }
        public bool ShowEmptyHearts { get; set; }
        public bool ShowPartialhearts { get; set; }
        public bool Centered { get; set; }
    }
}
