using NetworkPackets.Login.Domain;
using NetworkPackets.Shared;

namespace NetworkPackets.Login.Features;

public interface IShardList<TShard, TShardCollection>
    where TShard : IShard
    where TShardCollection : ICollection<TShard>
{
    internal void WriteShards<TData>(TShardCollection shards, TData data)
        where TData : IData
    {
        data.WriteByte(0xFF);

        data.WriteUShort((ushort)shards.Count);

        foreach (var shard in shards)
        {
            shard.WriteShardDescription(data);
        }
    }
}