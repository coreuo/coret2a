using Core.Abstract.Attributes;

namespace Compression;

[Entity("Data")]
public interface IData
{
    [Size(2048)]
    public Span<byte> Value { get; }

    public int Start { get; set; }

    public int Length { get; set; }
}