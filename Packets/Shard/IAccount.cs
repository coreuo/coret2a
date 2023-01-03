using Core.Abstract.Attributes;
using Packets.Shard.Outgoing;

namespace Packets.Shard;

[Entity("Account")]
public interface IAccount<TCharacter, out TCharacterCollection> :
    ICharacterList<TCharacter, TCharacterCollection>
    where TCharacter : IMobile
    where TCharacterCollection : ICollection<TCharacter>
{

}