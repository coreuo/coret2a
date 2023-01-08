namespace Packets.Shard.Features;

public interface IMode
{
    byte Mode { get; set; }

    internal void ReadMode<TData>(TData data)
        where TData : IData
    {
        Mode = data.ReadByte();
    }

    internal void WriteMode<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(Mode);
    }
}