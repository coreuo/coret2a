using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Accounting.Login;

[Entity("Login", "Server")]
public interface ILogin<in TState, TShard, TAccount, out TAccountCollection>
    where TState : IState<TShard, TAccount, TAccountCollection>
    where TShard : IShard<TAccount, TAccountCollection>
    where TAccount : IAccount
    where TAccountCollection : ICollection<TAccount>
{
    TAccountCollection Accounts { get; }

    [Priority(0.9)]
    public void OnPacketAccountLoginRequest(TState state)
    {
        var account = Accounts.SingleOrDefault(a => Is.Equal(a.Name, state.Name) && Is.Equal(a.Password, state.Password));

        if (Is.Default(account)) state.Reason = 0x00;

        else state.Account = account!;
    }

    [Priority(0.9)]
    public void OnPacketBritanniaSelect(TState state)
    {
        state.Account.GenerateAccessKey();

        state.Shard.Accounts.Add(state.Account);
    }
}