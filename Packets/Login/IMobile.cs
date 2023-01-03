using Core.Abstract.Attributes;

namespace Packets.Login;

[Entity("Mobile")]
public interface IMobile
{
    [Size(30)]
    Span<char> Name { get; }

    [Size(30)]
    Span<char> Password { get; }
}