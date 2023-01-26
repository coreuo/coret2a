using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface ITipRequest
{
    int Tip { set; }

    internal void ReadTipRequest<TData>(TData data)
        where TData : IData
    {
        Tip = data.ReadShort();
    }
}