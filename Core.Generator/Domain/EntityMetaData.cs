namespace Core.Generator.Domain
{
    public readonly struct EntityMetaData
    {
        public string FullName { get; }

        public string Name { get; }

        public bool Cached { get; }

        public bool Synchronized { get; }

        public EntityMetaData(string fullName, string name, bool cached, bool synchronized)
        {
            FullName = fullName;
            Name = name;
            Cached = cached;
            Synchronized = synchronized;
        }

        public string ToStoreProperty()
        {
            if(Cached) return $@"
    public Cache<{FullName}> {Name}Store {{ get; }}";
            else return $@"
    public Pool<Save, {Name}> {Name}Store {{ get; }}";
        }

        public string ToStoreAssignment(bool synchronized)
        {
            if (Cached) return $@"
        {Name}Store = new Cache<{FullName}>();";
            else return $@"
        {Name}Store = ReadPool<{Name}>(this, {synchronized.ToString().ToLower()});";
        }

        public string ToStoreFlush()
        {
            return $@"
        {Name}Store.Flush();";
        }
    }
}
