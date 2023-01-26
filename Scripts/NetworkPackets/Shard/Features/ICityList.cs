using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface ICityList
{
    internal void WriteCities<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(1);

        data.WriteByte(0);

        Span<char> buffer = stackalloc char[30];

        "Ocllo".CopyTo(buffer);

        data.WriteAscii(buffer, 31);

        "Bountiful Harvest".CopyTo(buffer);

        data.WriteAscii(buffer, 31);
    }
}