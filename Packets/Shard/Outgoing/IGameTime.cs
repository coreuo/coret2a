namespace Packets.Shard.Outgoing
{
    public interface IGameTime
    {
        internal void WriteGameTime<TData>(TData data)
            where TData : IData
        {
            data.WriteByte(0x10); //hour

            data.WriteByte(0x23); //minute

            data.WriteByte(0x12); //second
        }
    }
}
