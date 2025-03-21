namespace DialogueDisplayFramework.Data
{
    public class GiftsData : BaseData, IGiftsData
    {
        public bool ShowGiftIcon { get; set; }
        public bool Inline { get; set; }
        public GiftsData()
        {
            ShowGiftIcon = true;
        }
    }
}
