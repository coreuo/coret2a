using Core.Abstract.Attributes;

namespace NetworkPackets.Shard.Domain;

[Element("Skill")]
public interface ISkill
{
    ushort Value { get; }
}