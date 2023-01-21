using System.Net;
using System.Text;
using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Packets.Shared;

[Entity("Data")]
public interface IData
{
    public byte Packet { get; set; }

    [Length(2048)]
    public Span<byte> Value { get; }

    public int Start { get; set; }

    public int Offset { get; set; }

    public int Length { get; set; }

    internal byte BeginRead()
    {
        var value = Value[Start];

        Offset = 1;

        return value;
    }

    internal byte ReadByte()
    {
        var value = Value[Start + Offset];
#if DEBUG
        Debug($"{Offset:D3} BYTE    0x{value:X2} ({value})");
#endif
        Offset++;

        return value;
    }

    internal sbyte ReadSByte()
    {
        var value = (sbyte)Value[Start + Offset];
#if DEBUG
        Debug($"{Offset:D3} SBYTE   0x{value:X2} ({value})");
#endif
        Offset++;

        return value;
    }

    internal short ReadShort()
    {
        var value = (short)((Value[Start + Offset] << 8) | Value[Start + Offset + 1]);
#if DEBUG
        Debug($"{Offset:D3} SHORT   0x{value:X4} ({value})");
#endif
        Offset += 2;

        return value;
    }

    internal ushort ReadUShort()
    {
        var value = (ushort)((Value[Start + Offset] << 8) | Value[Start + Offset + 1]);
#if DEBUG
        Debug($"{Offset:D3} USHORT  0x{value:X4} ({value})");
#endif
        Offset += 2;

        return value;
    }

    internal int ReadInt()
    {
        var value = (Value[Start + Offset] << 24) |
                    (Value[Start + Offset + 1] << 16) |
                    (Value[Start + Offset + 2] << 8) |
                    Value[Start + Offset + 3];
#if DEBUG
        Debug($"{Offset:D3} INT     0x{value:X8} ({value})");
#endif
        Offset += 4;

        return value;
    }

    internal uint ReadUInt()
    {
        var value = ((uint)Value[Start + Offset] << 24) |
                     ((uint)Value[Start + Offset + 1] << 16) |
                     ((uint)Value[Start + Offset + 2] << 8) |
                    Value[Start + Offset + 3];
#if DEBUG
        Debug($"{Offset:D3} UINT    0x{value:X8} ({value})");
#endif
        Offset += 4;

        return value;
    }

    private static readonly Decoder AsciiDecoder = Encoding.ASCII.GetDecoder();

    internal void ReadAscii(Span<char> buffer, int length)
    {
        AsciiDecoder.Convert(Value.Slice(Start + Offset, length), buffer, true, out _, out _, out _);
#if DEBUG
        Debug($"{Offset:D3} ASCII   {buffer.AsText()}");
#endif
        Offset += length;
    }

    internal void ReadEnd()
    {
        var offset = Offset;
#if DEBUG
        Debug($"{offset:D3} END");
#endif
        Start += offset;

        Length -= offset;

        Offset = 0;
    }

    internal void BeginWrite(byte value)
    {
        Start = 0;

        Value[0] = value;

        Offset = 1;
    }

    internal void OffsetSize()
    {
        Offset = 3;
    }

    internal void WriteByte(byte value)
    {
#if DEBUG
        Debug($"{Offset:D3} BYTE    0x{value:X2} ({value})");
#endif
        Value[Start + Offset] = value;

        Offset++;
    }

    internal void WriteBool(bool value)
    {
#if DEBUG
        Debug($"{Offset:D3} BOOL    {(value ? "TRUE" : "FALSE")}");
#endif
        Value[Start + Offset] = value ? (byte)1 : (byte)0;

        Offset++;
    }

    internal void WriteSByte(sbyte value)
    {
#if DEBUG
        Debug($"{Offset:D3} SBYTE   0x{value:X2} ({value})");
#endif
        Value[Start + Offset] = (byte)value;

        Offset++;
    }

    internal void WriteShort(short value)
    {
#if DEBUG
        Debug($"{Offset:D3} SHORT   0x{value:X4} ({value})");
#endif
        Value[Start + Offset] = (byte)(value >> 8);
        Value[Start + Offset + 1] = (byte)value;

        Offset += 2;
    }

    internal void WriteUShort(ushort value)
    {
#if DEBUG
        Debug($"{Offset:D3} USHORT  0x{value:X4} ({value})");
#endif
        Value[Start + Offset] = (byte)(value >> 8);
        Value[Start + Offset + 1] = (byte)value;

        Offset += 2;
    }

    internal void WriteSize(ushort value)
    {
        Value[Start + 1] = (byte)(value >> 8);
        Value[Start + 2] = (byte)value;
    }

    internal void WriteInt(int value)
    {
#if DEBUG
        Debug($"{Offset:D3} INT     0x{value:X8} ({value})");
#endif
        Value[Start + Offset] = (byte)(value >> 24);
        Value[Start + Offset + 1] = (byte)(value >> 16);
        Value[Start + Offset + 2] = (byte)(value >> 8);
        Value[Start + Offset + 3] = (byte)value;

        Offset += 4;
    }

    internal void WriteUInt(uint value)
    {
#if DEBUG
        Debug($"{Offset:D3} UINT    0x{value:X8} ({value})");
#endif
        Value[Start + Offset] = (byte)(value >> 24);
        Value[Start + Offset + 1] = (byte)(value >> 16);
        Value[Start + Offset + 2] = (byte)(value >> 8);
        Value[Start + Offset + 3] = (byte)value;

        Offset += 4;
    }

    internal void WriteIpAddress(IPAddress address)
    {
#if DEBUG
        Debug($"{Offset:D3} IP      {address}");
#endif
        address.TryWriteBytes(Value.Slice(Start + Offset, 4), out _);

        Offset += 4;
    }

    private static readonly Encoder AsciiEncoder = Encoding.ASCII.GetEncoder();

    internal void WriteAscii(ReadOnlySpan<char> text, int size)
    {
#if DEBUG
        Debug($"{Offset:D3} ASCII   {text}");
#endif
        WriteText(AsciiEncoder, text, size);

        Offset += size;
    }

    private int WriteText(Encoder encoder, ReadOnlySpan<char> text, int? size = null, int terminated = 0)
    {
        encoder.Convert(text, Value[(Start + Offset)..], true, out _, out var length, out _);

        var next = size ?? length + terminated;

        for (var i = length; i < next; i++) Value[Start + Offset + i] = 0;

        return next;
    }

    internal int EndWrite()
    {
#if DEBUG
        Debug($"{Offset:D3} END");
#endif
        return Length = Offset;
    }
#if DEBUG
    private static void Debug(string text)
    {
        Console.WriteLine(text);
    }
#endif
}