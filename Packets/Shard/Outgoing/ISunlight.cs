namespace Packets.Shard.Outgoing
{
    public interface ISunlight
    {
        byte Sunlight { get; }

        internal void WriteSunlight<TData>(TData data)
            where TData : IData
        {
            data.WriteByte(Sunlight);
        }
    }
}
