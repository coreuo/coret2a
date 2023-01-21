using Core.Abstract.Attributes;
using Packets.Shard.Features;
using Packets.Shared.Features;

namespace Packets.Shard.Domain;

[Entity("Shard", "Account")]
public interface IAccount<TCharacter, out TCharacterCollection> :
    IName,
    IPassword,
    ICharacterList<TCharacter, TCharacterCollection>
    where TCharacter : IMobile
    where TCharacterCollection : ICollection<TCharacter>
{

}