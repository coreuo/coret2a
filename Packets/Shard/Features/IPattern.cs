namespace Packets.Shard.Features;

public interface IPattern
{
    internal void ReadPattern<TData>(TData data)
        where TData : IData
    {
        var pattern = data.ReadUInt();

        if (pattern != 0xEDEDEDED) throw new InvalidOperationException("Invalid packet pattern.");
    }
}