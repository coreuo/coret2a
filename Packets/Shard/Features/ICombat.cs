using Core.Abstract.Attributes;

namespace Packets.Shard.Features;

public interface ICombat
{
    [Flag("Status", 4)]
    bool Combat { get; }

    internal void WriteCombat<TData>(TData data)
        where TData : IData
    {
        data.WriteBool(Combat); //IsFighting

        data.WriteByte(0); //AutoFight

        data.WriteByte(0x32); //Aggressiveness

        data.WriteByte(0); //RetreatSensitivity
    }
}