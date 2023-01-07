namespace Packets.Shard.Features;

public interface ICharacterSlot
{
    int Slot { set; }

    internal void ReadCharacterPattern<TData>(TData data)
        where TData : IData
    {
        var pattern = data.ReadUInt();

        if (pattern != 0xEDEDEDED) throw new InvalidOperationException("Invalid packet pattern.");
    }

    internal void ReadCharacterSlot<TData>(TData data)
        where TData : IData
    {
        Slot = data.ReadInt();
    }
}