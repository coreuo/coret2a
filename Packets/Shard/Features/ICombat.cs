using Core.Abstract.Attributes;

namespace Packets.Shard.Features;

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

    internal void WriteCombat<TData>(TData data)
        where TData : IData
    {
        data.WriteBool(Combat); //IsFighting

        data.WriteByte(0); //AutoFight

        data.WriteByte(0x32); //Aggressiveness

        data.WriteByte(0); //RetreatSensitivity
    }
}