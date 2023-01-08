using Core.Abstract.Attributes;

namespace Packets.Shard.Features
{
    public interface ISkillList<TSkill, TSkillArray>
        where TSkill : ISkill
        where TSkillArray : IReadOnlyList<TSkill>
    {
        [Size(49)]
        TSkillArray Skills { get; }
    }
}
