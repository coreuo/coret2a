using Core.Abstract.Attributes;

namespace Packets.Shard.Domain;

[Element("Skill")]
public interface ISkill
{
    ushort Value { get; }
}