using Core.Abstract.Attributes;

namespace Accounting.Login;

[Entity("Shard", "Server")]
public interface IShard<TAccount, out TAccountCollection>
    where TAccount : IAccount
    where TAccountCollection : ICollection<TAccount>
{
    TAccountCollection Accounts { get; }
}