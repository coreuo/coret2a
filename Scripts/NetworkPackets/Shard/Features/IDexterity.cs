using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface IDexterity
{
    ushort Dexterity { get; set; }

    internal void ReadDexterity<TData>(TData data)
        where TData : IData
    {
        Dexterity = data.ReadUShort();
    }

    internal void WriteDexterity<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(Dexterity);
    }
}