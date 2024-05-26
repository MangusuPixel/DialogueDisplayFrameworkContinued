namespace DialogueDisplayFramework.Data
{
    public interface IDividerData : IBaseData
    {
        public string ID { get; set; }
        public bool Horizontal { get; set; }
        public bool Small { get; set; }
        public DividerConnectorData Connectors { get; set; }
        public string Color { get; set; }
    }
    public interface IDividerConnectorData
    {
        public bool Top { get; set; }
        public bool Bottom { get; set; }
    }
}
