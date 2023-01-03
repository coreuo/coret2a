using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Core.Abstract.Extensions;

public static class SpanExtensions
{
    public static unsafe ReadOnlySpan<char> AsText(this Span<char> span)
    {
        var p = Unsafe.AsPointer(ref span.GetPinnableReference());

        return MemoryMarshal.CreateReadOnlySpanFromNullTerminated((char*)p);
    }

    public static unsafe ReadOnlySpan<char> AsText(this ReadOnlySpan<char> span)
    {
        var reference = span.GetPinnableReference();

        var p = Unsafe.AsPointer(ref reference);

        return MemoryMarshal.CreateReadOnlySpanFromNullTerminated((char*)p);
    }
}