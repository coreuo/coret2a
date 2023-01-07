namespace Packets.Login.Features;

public interface IShardSelect<TShard>
    where TShard : IShard
{
    public TShard Shard { get; set; }

    TShard GetShard(int shardId);

    internal void ReadShard<TData>(TData data)
        where TData : IData
    {
        var id = data.ReadShort();

        Shard = GetShard(id);
    }
}