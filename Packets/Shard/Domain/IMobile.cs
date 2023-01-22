using Core.Abstract.Attributes;
using Packets.Shard.Features;
using Packets.Shared.Features;

namespace Packets.Shard.Domain;

[Entity("Mobile")]
public interface IMobile<TMap, TSkill, TSkillArray> : IMobile,
    ISkillList<TSkill, TSkillArray>
    where TMap : IMap
    where TSkill : ISkill
    where TSkillArray : IReadOnlyList<TSkill>
{
    TMap Map { get; }
}

[Entity("Mobile")]
public interface IMobile : 
    IProfile,
    IFullName,
    IPassword,
    ISunlight,
    ILight,
    IWeather,
    IDirection,
    INotoriety,
    IStatus,
    IId,
    IBody,
    ILocation,
    IHue,
    IGender,
    IPoison,
    IBlessed,
    ICombat,
    IHidden,
    IHits,
    IMode,
    IStrength,
    IDexterity,
    IIntelligence,
    IStamina,
    IMana,
    IGold,
    IArmor,
    IWeight
{
}