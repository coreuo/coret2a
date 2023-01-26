using Core.Abstract.Attributes;
using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface ICombat
{
    [Flag("Status", 6)]
    bool Combat { get; set; }

    internal void ReadCombat<TData>(TData data)
        where TData : IData
    {
        Combat = data.ReadByte() > 0;

        data.Offset += 3;
    }

    internal void WriteCombat<TData>(TData data, bool combat)
        where TData : IData
    {
        data.WriteBool(combat); //IsFighting

        data.WriteByte(0); //AutoFight

        data.WriteByte(0x32); //Aggressiveness

        data.WriteByte(0); //RetreatSensitivity
    }
}