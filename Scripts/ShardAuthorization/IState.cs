using Core.Abstract.Attributes;

namespace Scripts.ShardAuthorization;

[Entity("Shard", "State")]
public interface IState<TAccount>
    where TAccount : IAccount
{
    TAccount Account { get; set; }

    Span<char> Name { get; }

    Span<char> Password { get; }

    int AccessKey { get; }

    bool AuthorizationSuccess { set; }
}