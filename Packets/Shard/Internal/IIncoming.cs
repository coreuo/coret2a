using Packets.Attributes.Incoming;

// ReSharper disable once CheckNamespace
namespace Packets.Shard.Domain;

public partial interface IShard<out TLogin, in TState, TData, TAccount, TMobile, out TMobileCollection, TMap, TSkill, TSkillArray>
{
    [InternalPacket(0x00)]
    public void OnInternalAuthorization(TData data)
    {
        ReadName(data);

        ReadPassword(data);

        ReadAccessKey(data);
    }
}