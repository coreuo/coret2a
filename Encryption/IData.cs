using Core.Abstract.Attributes;

namespace Encryption;

[Entity("Data")]
public interface IData
{
    [Size(2048)]
    public Span<byte> Value { get; }

    public int Start { get; }

    public int Length { get; }
}