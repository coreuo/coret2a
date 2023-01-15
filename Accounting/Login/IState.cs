using Core.Abstract.Attributes;

namespace Accounting.Login;

[Entity("Login", "State")]
public interface IState<TShard, TAccount>
    where TShard : IShard
    where TAccount : IAccount
{
    TAccount Account { get; set; }

    TShard Shard { get; set; }

    Span<char> Name { get; }

    Span<char> Password { get; }

    byte Status { set; }
}