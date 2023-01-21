using Core.Abstract.Attributes;
using Packets.Shard.Features;
using Packets.Shared;
using Packets.Shared.Features;

namespace Packets.Shard.Domain;

[Entity("Shard", "Server")]
public partial interface IShard<out TLogin, in TState, TData, TAccount, TMobile, out TMobileCollection, TMap, TSkill, TSkillArray> :
    ITransfer<TData>,
    ICityList,
    IGameTime,
    IName,
    IPassword,
    IAccessKey
    where TLogin : ILogin<TData>
    where TState : IState<TData, TAccount, TMobile, TMobileCollection, TMap, TSkill, TSkillArray>
    where TData : IData
    where TAccount : IAccount<TMobile, TMobileCollection>
    where TMobile : IMobile<TMap, TSkill, TSkillArray>
    where TMobileCollection : ICollection<TMobile>
    where TMap : IMap
    where TSkill : ISkill
    where TSkillArray : IReadOnlyList<TSkill>
{
#if DEBUG
    string Identity { get; }
#endif
    [Link("LoginServer.Shards.Owner")]
    TLogin Login { get; }

    [Priority(2.0)]
    public void OnInternalSendToLogin(TData data)
    {
        Login.SendInternal(data);
#if DEBUG
        Debug($"internal sent {data.Length} bytes to login");
#endif
    }
#if DEBUG
    private void Debug(string text)
    {
        Console.WriteLine($"[{Identity}] {text}");
    }
#endif
}