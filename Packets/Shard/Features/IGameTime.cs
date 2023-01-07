namespace Packets.Shard.Features;

public interface IGameTime
{
    internal void WriteGameTime<TData>(TData data)
        where TData : IData
    {
        var time = DateTime.Now;

        data.WriteByte((byte)time.Hour); //hour

        data.WriteByte((byte)time.Minute); //minute

        data.WriteByte((byte)time.Second); //second
    }
}