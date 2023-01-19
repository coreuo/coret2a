namespace Packets.Shard.Features;

public interface IMana
{
    ushort Mana { get; }

    ushort ManaMaximum { get; }

    internal void WriteMana<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(Mana);

        data.WriteUShort(ManaMaximum);
    }
}