using Packets.Shared;

namespace Packets.Shard.Features;

public interface IWeather
{
    internal void WriteWeather<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(0xFE); //Type

        data.WriteByte(0x0); //Particles

        data.WriteByte(0x0); //Temperature
    }
}