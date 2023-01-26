using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface ICharacterSlot
{
    uint Slot { set; }

    internal void ReadCharacterSlot<TData>(TData data)
        where TData : IData
    {
        Slot = data.ReadUInt();
    }
}