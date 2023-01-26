using Core.Abstract.Attributes;

namespace Scripts.NetworkEncryption;

[Entity("Data")]
public interface IData
{
    public Span<byte> Value { get; }

    public int Start { get; }

    public int Length { get; }
}