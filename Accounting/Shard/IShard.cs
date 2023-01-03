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
    TAccountCollection Accounts { get; }

    [Priority(0.9)]
    public void OnPacketPostLogin(TState state)
    {
        var account = Accounts.SingleOrDefault(a => a.AccessKey == state.AccessKey && Is.Equal(a.Name, state.Name) && Is.Equal(a.Password, state.Password));

        if (Is.Default(account)) state.Reason = 0x00;

        else state.Account = account!;
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
        Accounts.Remove(state.Account);
    }
}