using Core.Abstract.Attributes;

namespace Server.Shard;

[Entity("Shard", "State")]
public interface IState<out TAccount, TMobile>
    where TAccount : IAccount
    where TMobile : IMobile
{
    TAccount Account { get; }

    TMobile Character { get; }

    [Size(60)]
    Span<char> Name { get; }

    int Serial { get; }

    internal void TransferName()
    {
        Character.Name.CopyTo(Name);
    }

    internal bool IsSelfTargeted()
    {
        return Serial == Character.Id;
    }
}