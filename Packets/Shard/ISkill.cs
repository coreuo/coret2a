using Core.Abstract.Attributes;

namespace Packets.Shard;

[Element("Skill")]
public interface ISkill
{
    ushort Value { get; }

    ushort Base { get; }

    ushort Cap { get; }

    byte Status { get; }
}