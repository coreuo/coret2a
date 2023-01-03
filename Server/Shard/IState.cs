using Core.Abstract.Attributes;

namespace Server.Shard;

[Entity("Shard", "State")]
public interface IState<out TAccount>
    where TAccount : IAccount
{
    TAccount Account { get; }
}