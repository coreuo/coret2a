using Core.Abstract.Attributes;

namespace Accounting.Shard;

[Entity("Shard", "State")]
public interface IState<TAccount, in TCharacter, TCharacterCollection>
    where TAccount : IAccount<TCharacter, TCharacterCollection>
    where TCharacter : ICharacter
    where TCharacterCollection : ICollection<TCharacter>
{
    TAccount Account { get; set; }

    TCharacter Character { set; }

    [Size(30)]
    Span<char> Name { get; }

    [Size(30)]
    Span<char> Password { get; }

    int AccessKey { get; }

    byte Status { set; }

    int Slot { get; }
}