using Core.Abstract.Attributes;

namespace Network;

[Entity("Data")]
[Synchronized]
public interface IData
{
    [Size(2048)]
    Span<byte> Value { get; }

    int Start { get; set; }

    int Length { get; set; }

    internal void Reset()
    {
        Start = 0;

        Length = Value.Length;
    }
}