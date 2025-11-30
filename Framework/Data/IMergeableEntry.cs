namespace DialogueDisplayFramework.Data
{
    public interface IMergeableEntry<T>
    {
        string ID { get; }
        public void MergeFrom(T other);
    }
}
