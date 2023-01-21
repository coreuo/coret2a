using Packets.Attributes.Outgoing;

// ReSharper disable once CheckNamespace
namespace Packets.Shard.Domain;

public partial interface IShard<out TLogin, in TState, TData, TAccount, TMobile, out TMobileCollection, TMap, TSkill, TSkillArray>
{
    [InternalLoginPacket(0x01)]
    public void OnInternalAccountOnline(TState state, TData data)
    {
        state.Account.WriteName(data);
    }

    [InternalLoginPacket(0x02)]
    public void OnInternalAccountOffline(TState state, TData data)
    {
        state.Account.WriteName(data);
    }
}