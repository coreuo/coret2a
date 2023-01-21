using Packets.Shared;

namespace Packets.Shard.Features;

public interface ILocation
{
    ushort X { get; set; }

    ushort Y { get; set; }

    sbyte Z { get; set; }

    internal void WriteX<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(X);
    }

    internal void ReadX<TData>(TData data)
        where TData : IData
    {
        X = data.ReadUShort();
    }

    internal void WriteY<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(Y);
    }

    internal void ReadY<TData>(TData data)
        where TData : IData
    {
        Y = data.ReadUShort();
    }

    internal void WriteSByteZ<TData>(TData data)
        where TData : IData
    {
        data.WriteSByte(Z);
    }

    internal void WriteShortZ<TData>(TData data)
        where TData : IData
    {
        data.WriteShort(Z);
    }

    internal void ReadSByteZ<TData>(TData data)
        where TData : IData
    {
        Z = data.ReadSByte();
    }

    internal void ReadShortZ<TData>(TData data)
        where TData : IData
    {
        Z = (sbyte)data.ReadShort();
    }
}