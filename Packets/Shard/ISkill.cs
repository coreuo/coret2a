using Core.Abstract.Attributes;

namespace Packets.Shard;

[Element("Skill")]
public interface ISkill
{
    int Id { get; }

    ushort Value { get; }
}