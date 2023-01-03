using Core.Abstract.Attributes;

namespace Accounting.Login;

[Entity("Login", "State")]
public interface IState<TShard, TAccount, TAccountCollection>
    where TShard : IShard<TAccount, TAccountCollection>
    where TAccount : IAccount
    where TAccountCollection : ICollection<TAccount>
{
    TAccount Account { get; set; }

    TShard Shard { get; set; }

    [Size(30)]
    Span<char> Name { get; }

    [Size(30)]
    Span<char> Password { get; }

    byte Reason { set; }
}