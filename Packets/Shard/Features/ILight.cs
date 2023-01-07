namespace Packets.Shard.Features;

public interface ILight
{
    byte Light { get; }

    internal void WriteLight<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(Light);
    }
}