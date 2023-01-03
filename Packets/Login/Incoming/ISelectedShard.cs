namespace Packets.Login.Incoming;

public interface ISelectedShard<TShard>
    where TShard : IShard
{
    public TShard Shard { get; set; }

    TShard GetShard(int shardId);

    internal void ReadSelectedShard<TData>(TData data)
        where TData : IData
    {
        var id = data.ReadShort();

        Shard = GetShard(id);
    }
}