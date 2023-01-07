namespace Packets.Shard.Features;

public interface ICityList
{
    internal void WriteCities<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(1);

        data.WriteByte(0);

        data.WriteAscii("Ocllo".AsSpan(), 31);

        data.WriteAscii("Bountiful Harvest".AsSpan(), 31);
    }
}