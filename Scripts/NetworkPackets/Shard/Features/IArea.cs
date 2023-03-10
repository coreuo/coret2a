using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface IArea :
    IId
{
    ushort Left { get; }

    ushort Top { get; }

    ushort Width { get; }

    ushort Height { get; }

    internal void WriteByteAreaId<TData>(TData data)
        where TData : IData
    {
        data.WriteByte((byte)Id);
    }

    internal void WriteUShortAreaId<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort((ushort)Id);
    }

    internal void WriteArea<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(Left);

        data.WriteUShort(Top);

        data.WriteUShort(Width);

        data.WriteUShort(Height);
    }
}