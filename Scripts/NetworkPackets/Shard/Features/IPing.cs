using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface IPing
{
    byte Ping { get; set; }

    internal void ReadPing<TData>(TData data)
        where TData : IData
    {
        Ping = data.ReadByte();
    }

    internal void WritePing<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(Ping);
    }
}