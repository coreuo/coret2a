namespace Packets.Shard.Features;

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
        data.WriteByte((byte)0x71);
    }

    internal void WriteUShortAreaId<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort((ushort)0x71);
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