using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface INotoriety
{
    byte Notoriety { get; set; }

    internal void ReadNotoriety<TData>(TData data)
        where TData : IData
    {
        Notoriety = data.ReadByte();
    }

    internal void WriteNotoriety<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(0x1);
    }
}