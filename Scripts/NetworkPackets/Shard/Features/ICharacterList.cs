using NetworkPackets.Shard.Domain;
using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

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

            data.Offset += 30;

            //data.WriteAscii("", 30);

            index++;
        }

        data.Offset += (5 - index) * 60;
    }
}