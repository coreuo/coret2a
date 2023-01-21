using Packets.Shared;

namespace Packets.Shard.Features;

public interface IStrength
{
    ushort Strength { get; set; }

    internal void ReadStrength<TData>(TData data)
        where TData : IData
    {
        Strength = data.ReadUShort();
    }

    internal void WriteStrength<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(Strength);
    }
}