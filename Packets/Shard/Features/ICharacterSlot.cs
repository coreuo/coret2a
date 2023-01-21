using Packets.Shared;

namespace Packets.Shard.Features;

public interface ICharacterSlot
{
    int Slot { set; }

    internal void ReadCharacterSlot<TData>(TData data)
        where TData : IData
    {
        Slot = data.ReadInt();
    }
}