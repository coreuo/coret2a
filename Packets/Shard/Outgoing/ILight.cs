namespace Packets.Shard.Outgoing
{
    public interface ILight
    {
        byte Light { get; }

        internal void WriteLight<TData>(TData data)
            where TData : IData
        {
            data.WriteByte(Light);
        }
    }
}
