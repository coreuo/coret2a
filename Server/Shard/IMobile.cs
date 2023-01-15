using Core.Abstract.Attributes;

namespace Server.Shard;

[Entity("Mobile")]
public interface IMobile
{
    int Id { get; }

    Span<char> Name { get; }

    bool Combat { set; }
}