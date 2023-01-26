using Core.Abstract.Attributes;

namespace Scripts.NetworkServer;

[Entity("Data")]
[Synchronized]
public interface IData
{
    Span<byte> Value { get; }

    int Start { get; set; }

    int Length { get; set; }

    internal void Reset()
    {
        Start = 0;

        Length = Value.Length;
    }
}