namespace DialogueDisplayFramework.Data
{
    public class DividerData : BaseData, IDividerData
    {
        public string ID { get; set; }
        public bool Horizontal { get; set; }
        public bool Small { get; set; }
        public DividerConnectorData Connectors { get; set; }
        public string Color { get; set; }
        public DividerData()
        {
            ID = DisplayDataUtils.MISSING_ID_STR;
            Connectors = new()
            {
                Top = true,
                Bottom = true
            };
        }
    }
    public class DividerConnectorData : IDividerConnectorData
    {
        public bool Top { get; set; }
        public bool Bottom { get; set; }
    }
}
