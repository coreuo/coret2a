using Core.Abstract.Attributes;
using Core.Abstract.Extensions;
using Scripts.LoginAuthentication;

namespace Scripts.AccountLogin;

[Entity("Login", "Server")]
public interface ILogin<TState, TAccount, out TAccountCollection>
    where TState : IState<TAccount>
    where TAccount : IAccount
    where TAccountCollection : ICollection<TAccount>
{
#if DEBUG
    string Identity { get; }
#endif
    Span<char> Name { get; }

    TAccountCollection Accounts { get; }

    TAccountCollection Initiated { get; }

    TAccountCollection Online { get; }

    void PacketAccountLoginFailed(TState state);

    [Priority(1.0)]
    public void OnPacketAccountLoginRequest(TState state)
    {
        var success = state.AuthenticationSuccess = TryAuthenticate(state);

        if (!success) PacketAccountLoginFailed(state);
    }

    private bool TryAuthenticate(TState state)
    {
        var account = Accounts.SingleOrDefault(a => Is.Equal(a.Name, state.Name))!;

        if (Is.Default(account)) state.Status = 0x00;

        else if (Initiated.Contains(account) || Online.Contains(account)) state.Status = 0x01;

        else if (!Is.Equal(account.Password, state.Password)) state.Status = 0x03;

        else
        {
#if DEBUG
            Debug($"{account.Name} authenticated");
#endif
            Initiated.Add(state.Account = account);

            return true;
        }
#if DEBUG
        Debug($"{account.Name} authentication failed (reason {state.Status})");
#endif
        return false;
    }

    [Priority(1.0)]
    public void OnInternalAccountOnline()
    {
        var account = Accounts.Single(a => Is.Equal(a.Name, Name));
#if DEBUG
        Debug($"{account.Name} is online");
#endif
        Online.Add(account);
    }

    [Priority(1.0)]
    public void OnInternalAccountOffline()
    {
        var account = Accounts.Single(a => Is.Equal(a.Name, Name));
#if DEBUG
        Debug($"{account.Name} is offline");
#endif
        Online.Remove(account);
    }

    [Priority(1.0)]
    public void OnDisconnected(TState state)
    {
#if DEBUG
        Debug($"{state.Account.Name} left login server");
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