using Core.Abstract.Attributes;
using Server.Login;

namespace Server.Shard;

[Entity("Shard", "State")]
public interface IState<TAccount>
    where TAccount : IAccount
{
    TAccount Account { get; }
}