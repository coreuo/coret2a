namespace Packets.Shard.Features;

public interface ILocation
{
    ushort X { get; set; }

    ushort Y { get; set; }

    sbyte Z { get; set; }

    internal void ReadLocation<TData>(TData data)
        where TData : IData
    {
        ReadX(data);

        ReadY(data);

        ReadZ(data);
    }

    internal void WriteLocation<TData>(TData data)
        where TData : IData
    {
        WriteX(data);

        WriteY(data);

        WriteZ(data);
    }

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

    internal void WriteZ<TData>(TData data)
        where TData : IData
    {
        data.WriteSByte(Z);
    }

    internal void ReadZ<TData>(TData data)
        where TData : IData
    {
        Z = data.ReadSByte();
    }
}