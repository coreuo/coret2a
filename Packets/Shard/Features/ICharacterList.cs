using Packets.Shard.Domain;
using Packets.Shared;

namespace Packets.Shard.Features;

public interface ICharacterList<TCharacter, out TCharacterCollection>
    where TCharacter : IMobile
    where TCharacterCollection : ICollection<TCharacter>
{
    TCharacterCollection Characters { get; }

    internal void WriteCharacters<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(5);

        var index = 0;

        foreach (var character in Characters)
        {
            character.WriteName(data);

            data.WriteAscii("", 30);

            index++;
        }

        data.Offset += (5 - index) * 60;
    }
}