using Core.Abstract.Attributes;

namespace Accounting.Login;

[Entity("Login", "State")]
public interface IState<TShard, TAccount>
    where TShard : IShard
    where TAccount : IAccount
{
    TAccount Account { get; set; }

    TShard Shard { get; set; }

    [Size(30)]
    Span<char> Username { get; }

    [Size(30)]
    Span<char> Password { get; }

    byte Status { set; }
}