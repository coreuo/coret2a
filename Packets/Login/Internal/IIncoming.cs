using Packets.Attributes.Incoming;

// ReSharper disable once CheckNamespace
namespace Packets.Login.Domain;

public partial interface ILogin<TState, TData, TShard, out TShardCollection, TAccount>
{
    [InternalPacket(0x01)]
    public void OnInternalAccountOnline(TData data)
    {
        ReadName(data);
    }

    [InternalPacket(0x02)]
    public void OnInternalAccountOffline(TData data)
    {
        ReadName(data);
    }
}