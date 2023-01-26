using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface IDirection
{
    byte Direction { get; set; }

    internal void ReadDirection<TData>(TData data)
        where TData : IData
    {
        Direction = data.ReadByte();
    }

    internal void WriteDirection<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(Direction);
    }
}