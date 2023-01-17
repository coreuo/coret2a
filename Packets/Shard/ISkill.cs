using Core.Abstract.Attributes;

namespace Packets.Shard;

[Element("Skill")]
public interface ISkill
{
    ushort Value { get; }
}