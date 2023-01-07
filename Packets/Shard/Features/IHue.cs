namespace Packets.Shard.Features;

public interface IHue
{
    ushort Hue { get; }

    internal void WriteHue<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(Hue);
    }
}