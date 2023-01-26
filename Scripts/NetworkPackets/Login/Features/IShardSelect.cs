using NetworkPackets.Shared;

namespace NetworkPackets.Login.Features;

public interface IShardSelect
{
    ushort ShardId { set; }

    internal void ReadShard<TData>(TData data)
        where TData : IData
    {
        ShardId = data.ReadUShort();
    }
}