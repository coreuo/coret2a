using Packets.Shared;

namespace Packets.Shard.Features;

public interface IStamina
{
    ushort Stamina { get; }

    ushort StaminaMaximum { get; }

    internal void WriteStamina<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(Stamina);

        data.WriteUShort(StaminaMaximum);
    }
}