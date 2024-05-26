namespace DialogueDisplayFramework.Data
{
    public interface IGiftsData : IBaseData
    {
        public bool ShowGiftIcon { get; set; }
        public bool Inline { get; set; }
    }
}
