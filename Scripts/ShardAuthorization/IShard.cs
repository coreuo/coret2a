using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Scripts.ShardAuthorization;

[Entity("Shard", "Server")]
public interface IShard<in TState, TAccount, out TAccountCollection>
    where TState : IState<TAccount>
    where TAccount : IAccount
    where TAccountCollection : ICollection<TAccount>
{
    Span<char> Name { get; }

    Span<char> Password { get; }

    int AccessKey { get; }

    TAccountCollection Accounts { get; }

    TAccountCollection Online { get; }

    TAccount LeaseAccount();

    void InternalAccountOnline(TState state);

    void InternalAccountOffline(TState state);

    [Priority(1.0)]
    public void OnInternalShardAuthorization()
    {
        var account = Accounts.SingleOrDefault(a => Is.Equal(a.Name, Name))!;

        if (Is.Default(account)) account = CreateShardAccount();
        
        Password.CopyTo(account.Password);

        account.AccessKey = AccessKey;
    }

    private TAccount CreateShardAccount()
    {
        var account = LeaseAccount();

        Name.CopyTo(account.Name);

        Accounts.Add(account);

        return account;
    }

    [Priority(1.0)]
    public void OnPacketPostLogin(TState state)
    {
        state.AuthorizationSuccess = TryAuthorize(state);
    }

    private bool TryAuthorize(TState state)
    {
        if (state.AccessKey == 0) return false;

        var account = Accounts.SingleOrDefault(a => a.AccessKey == state.AccessKey && Is.Equal(a.Name, state.Name) && Is.Equal(a.Password, state.Password))!;

        if (Is.Default(account)) return false;

        state.Account = account;

        Online.Add(account);

        InternalAccountOnline(state);

        return true;
    }

    [Priority(1.0)]
    public void OnDisconnected(TState state)
    {
        Online.Remove(state.Account);

        InternalAccountOffline(state);

        state.Account = default!;
    }
}