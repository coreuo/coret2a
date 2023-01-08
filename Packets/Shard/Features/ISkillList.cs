using Core.Abstract.Attributes;

namespace Packets.Shard.Features
{
    public interface ISkillList<TSkill, TSkillArray>
        where TSkill : ISkill
        where TSkillArray : IReadOnlyList<TSkill>
    {
        [Size(49)]
        TSkillArray Skills { get; }

        internal void WriteSkills<TData>(TData data)
            where TData : IData
        {
            foreach (var skill in Skills)
            {
                data.WriteUShort((ushort)skill.Id);

                data.WriteUShort(skill.Value);
            }
        }
    }
}
