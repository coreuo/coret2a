namespace Packets.Shard.Outgoing;

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
            character.WriteCredentials(data);

            index++;
        }

        data.Offset += (5 - index) * 60;
    }
}