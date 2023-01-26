using Core.Abstract.Attributes;
using NetworkPackets.Shard.Domain;
using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface ISkillList<TSkill, TSkillArray>
    where TSkill : ISkill
    where TSkillArray : IReadOnlyList<TSkill>
{
    [Length(49)]
    TSkillArray Skills { get; }

    internal void WriteSkills<TData>(TData data)
        where TData : IData
    {
        var index = 1;

        foreach (var skill in Skills)
        {
            data.WriteUShort((ushort)index);

            data.WriteUShort(skill.Value);

            index++;
        }
    }
}