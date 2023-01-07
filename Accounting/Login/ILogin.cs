using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Accounting.Login;

[Entity("Login", "Server")]
public interface ILogin<TState, TShard, TAccount, out TAccountCollection>
    where TState : IState<TShard, TAccount>
    where TShard : IShard
    where TAccount : IAccount
    where TAccountCollection : ICollection<TAccount>
{
#if DEBUG
    string Identity { get; }
#endif
    [Size(30)] Span<char> Name { get; }

    TAccountCollection Accounts { get; }

    TAccountCollection Initiated { get; }

    TAccountCollection Online { get; }

    void InternalShardAuthorization(TState state);

    [Priority(0.9)]
    public void OnPacketAccountLoginRequest(TState state)
    {
        var account = Accounts.SingleOrDefault(a => Is.Equal(a.Name, state.Name) && Is.Equal(a.Password, state.Password))!;

        if (Is.Default(account)) state.Status = 0x00;

        else if (Initiated.Contains(account) || Online.Contains(account)) state.Status = 0x01;

        else
        {
#if DEBUG
            Debug($"{account.Name} initiated login");
#endif
            Initiated.Add(state.Account = account);
        }
    }

    [Priority(0.9)]
    public void OnPacketBritanniaSelect(TState state)
    {
        state.Account.GenerateAccessKey();

        InternalShardAuthorization(state);
    }

    [Priority(1.0)]
    public void OnInternalShardAccountOnline()
    {
        var account = Accounts.Single(a => Is.Equal(a.Name, Name));
#if DEBUG
        Debug($"{account.Name} is online");
#endif
        Online.Add(account);
    }

    [Priority(1.0)]
    public void OnInternalShardAccountOffline()
    {
        var account = Accounts.Single(a => Is.Equal(a.Name, Name));
#if DEBUG
        Debug($"{account.Name} is offline");
#endif
        Online.Remove(account);
    }

    [Priority(0.9)]
    public void OnDisconnected(TState state)
    {
#if DEBUG
        Debug($"{state.Account.Name} disposed login");
#endif
        Initiated.Remove(state.Account);

        state.Account = default!;
    }
#if DEBUG
    private void Debug(string text)
    {
        Console.WriteLine($"[{Identity}] {text}");
    }
#endif
}