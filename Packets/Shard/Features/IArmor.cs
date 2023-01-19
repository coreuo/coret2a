namespace Packets.Shard.Features;

public interface IArmor
{
    ushort Armor { get; set; }

    internal void ReadArmor<TData>(TData data)
        where TData : IData
    {
        Armor = data.ReadUShort();
    }

    internal void WriteArmor<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(Armor);
    }
}