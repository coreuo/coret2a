using Core.Abstract.Attributes;

namespace Server.Shard;

[Entity("Mobile")]
public interface IMobile
{
    int Id { get; }

    [Size(60)]
    Span<char> Name { get; }
}