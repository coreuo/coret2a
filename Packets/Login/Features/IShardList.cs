namespace Packets.Login.Features;

public interface IShardList<TShard, out TShardCollection>
    where TShard : IShard
    where TShardCollection : ICollection<TShard>
{
    public TShardCollection Shards { get; }

    internal void WriteShards<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(0xFF);

        data.WriteShort((short)Shards.Count);

        foreach (var shard in Shards)
        {
            shard.WriteShardDescription(data);
        }
    }
}