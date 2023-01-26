using NetworkPackets.Shared;

namespace NetworkPackets.Login.Features;

public interface IAccessKey
{
    int AccessKey { get; }

    internal void WriteAccessKey<TData>(TData data)
        where TData : IData
    {
        data.WriteInt(AccessKey);
    }
}