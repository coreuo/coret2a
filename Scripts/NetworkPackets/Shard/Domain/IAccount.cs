using Core.Abstract.Attributes;
using NetworkPackets.Shard.Features;
using NetworkPackets.Shared.Features;

namespace NetworkPackets.Shard.Domain;

[Entity("Shard", "Account")]
public interface IAccount<TCharacter, out TCharacterCollection> :
    IName,
    IPassword,
    ICharacterList<TCharacter, TCharacterCollection>
    where TCharacter : IMobile
    where TCharacterCollection : ICollection<TCharacter>
{

}