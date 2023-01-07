using Core.Abstract.Attributes;
using Packets.Shard.Features;

namespace Packets.Shard;

[Entity("Shard", "Account")]
public interface IAccount<TCharacter, out TCharacterCollection> :
    IUsername,
    ICharacterList<TCharacter, TCharacterCollection>
    where TCharacter : IMobile
    where TCharacterCollection : ICollection<TCharacter>
{

}