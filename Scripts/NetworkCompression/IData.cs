using Core.Abstract.Attributes;

namespace Scripts.NetworkCompression;

[Entity("Data")]
public interface IData
{
    public Span<byte> Value { get; }

    public int Start { get; set; }

    public int Length { get; set; }
}