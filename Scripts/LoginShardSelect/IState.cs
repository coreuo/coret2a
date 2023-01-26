using Core.Abstract.Attributes;

namespace Scripts.LoginShardSelect;

[Entity("Login", "State")]
public interface IState<TAccount, TShard>
    where TAccount : IAccount
    where TShard : IShard
{
    TAccount Account { get; set; }

    TShard Shard { get; set; }

    ushort ShardId { get; }

    bool Success { get; }
}