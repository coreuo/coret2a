using Packets.Attributes.Outgoing;

// ReSharper disable once CheckNamespace
namespace Packets.Login.Domain;

public partial interface ILogin<TState, TData, TShard, out TShardCollection, TAccount>
{
    [InternalShardPacket(0x00)]
    public void OnInternalAuthorization(TState state, TData data)
    {
        state.Account.WriteName(data);

        state.Account.WritePassword(data);

        state.Account.WriteAccessKey(data);
    }
}