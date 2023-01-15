using Core.Abstract.Attributes;
using Packets.Server.Features;
using Packets.Shard.Features;

namespace Packets.Shard;

[Entity("Shard", "Account")]
public interface IAccount<TCharacter, out TCharacterCollection> :
    IName,
    IPassword,
    ICharacterList<TCharacter, TCharacterCollection>
    where TCharacter : IMobile
    where TCharacterCollection : ICollection<TCharacter>
{

}