using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Scripts.LoginShardSelect;

[Entity("Login", "Server")]
public interface ILogin<TState, TAccount, TShard>
    where TState : IState<TAccount, TShard>
    where TShard : IShard
    where TAccount : IAccount
{
    public TShard GetShard(int id);

    void PacketUserServer(TState state, TShard shard, TAccount account);

    void InternalShardAuthorization(TState state, TShard shard, TAccount account);

    [Priority(1.0)]
    public void OnPacketBritanniaSelect(TState state)
    {
        var shard = state.Shard = GetShard(state.ShardId);

        var account = state.Account;

        if (Is.Default(shard) || Is.Default(account)) return;

        account.AccessKey = Utility.Random.Next() | (Utility.Random.Next() % 2 > 0 ? 1 << 31 : 0);

        InternalShardAuthorization(state, shard, account);

        PacketUserServer(state, shard, account);
    }
}