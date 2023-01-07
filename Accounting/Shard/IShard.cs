using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Accounting.Shard;

[Entity("Shard", "Server")]
public interface IShard<in TState, TAccount, out TAccountCollection, TCharacter, TCharacterCollection>
    where TState : IState<TAccount, TCharacter, TCharacterCollection>
    where TAccount : IAccount<TCharacter, TCharacterCollection>
    where TAccountCollection : ICollection<TAccount>
    where TCharacter : ICharacter
    where TCharacterCollection : ICollection<TCharacter>
{
#if DEBUG
    string Identity { get; }
#endif
    [Size(30)] Span<char> Username { get; }

    [Size(30)] Span<char> Password { get; }

    int AccessKey { get; }

    TAccountCollection Accounts { get; }

    TAccountCollection Online { get; }

    TAccount LeaseAccount();

    void InternalShardAccountOnline(TState state);

    void InternalShardAccountOffline(TState state);

    [Priority(0.9)]
    public void OnInternalShardAuthorization()
    {
        var account = Accounts.SingleOrDefault(a => Is.Equal(a.Username, Username))!;

        if (Is.Default(account))
        {
            account = LeaseAccount();

            Username.CopyTo(account.Username);
        }
        
        Password.CopyTo(account.Password);

        account.AccessKey = AccessKey;
    }

    [Priority(0.9)]
    public void OnPacketPostLogin(TState state)
    {
        var account = Accounts.SingleOrDefault(a => a.AccessKey == state.AccessKey && Is.Equal(a.Username, state.Name) && Is.Equal(a.Password, state.Password))!;

        if (Is.Default(account) || account.AccessKey == 0) state.Account = default!;

        else
        {
            state.Account = account;

            Online.Add(account);

            InternalShardAccountOnline(state);
        }
    }

    [Priority(0.9)]
    public void OnPacketPreLogin(TState state)
    {
        if (state.Slot < state.Account.Characters.Count)
            state.Character = state.Account.Characters.ElementAt(state.Slot);
    }

    [Priority(0.9)]
    public void OnDisconnected(TState state)
    {
        Online.Remove(state.Account);

        InternalShardAccountOffline(state);

        state.Account = default!;
    }
}