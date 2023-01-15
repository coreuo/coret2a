﻿using Core.Abstract.Attributes;

namespace Server.Shard;

[Entity("Shard", "State")]
public interface IState<out TAccount, TMobile>
    where TAccount : IAccount
    where TMobile : IMobile
{
    TAccount Account { get; }

    TMobile Character { get; }

    [Size(30)]
    Span<char> Name { get; }

    int Target { get; }

    byte Mode { get; }

    [Flag("Status", 6)]
    bool Combat { get; }

    internal void TransferName()
    {
        Character.Name.CopyTo(Name);
    }

    internal void TransferCombat()
    {
        var character = Character;

        character.Combat = Combat;
    }

    internal bool IsSelfTargeted()
    {
        return Target == Character.Id;
    }
}