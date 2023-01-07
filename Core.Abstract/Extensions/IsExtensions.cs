using System.Runtime.CompilerServices;

namespace Core.Abstract.Extensions;

public static class Is
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Default<T>(T value)
    {
        return EqualityComparer<T>.Default.Equals(value, default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(ReadOnlySpan<char> x, ReadOnlySpan<char> y)
    {
        return x.AsText().Equals(y.AsText(), StringComparison.InvariantCulture);
    }
}