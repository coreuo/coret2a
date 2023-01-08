using System.Runtime.CompilerServices;
using Core.Abstract.Domain;
using Core.Launcher.Domain;

namespace Core.Launcher.ElementExtensions;

public static class ElementExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue GetValue<TElement, TValue>(this TElement element, IStore<TValue> store, int index)
        where TElement : IElement
    {
        var id = element.GetInt32(index);

        return store.GetValue(id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetValue<TElement, TValue>(this TElement element, IStore<TValue> store, int index, TValue value)
        where TElement : IElement
    {
        var id = store.GetId(value);

        element.SetInt32(index, id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span<TValue> GetSpan<TElement, TValue>(this TElement element, int index, int length)
        where TValue : struct
        where TElement : IElement
    {
        var pointer = element.GetPointer(index);

        return new Span<TValue>(pointer, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int ExchangeInt32<TElement>(this TElement element, int index, int value)
        where TElement : IElement
    {
        var p = element.GetInt32Pointer(index);

        var result = *p;

        *p = value;

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool GetFlag<TElement>(this TElement element, int index, int flag)
        where TElement : IElement
    {
        return (*element.GetBytePointer(index) & (1 << flag)) > 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetFlag<TElement>(this TElement element, int index, int flag, bool value)
        where TElement : IElement
    {
        *element.GetBytePointer(index) |= (byte)(1 << flag);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe sbyte GetSByte<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return *element.GetSBytePointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetSByte<TElement>(this TElement element, int index, sbyte value)
        where TElement : IElement
    {
        *element.GetSBytePointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe sbyte* GetSBytePointer<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return (sbyte*)element.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ushort GetUInt16<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return *element.GetUInt16Pointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetUInt16<TElement>(this TElement element, int index, ushort value)
        where TElement : IElement
    {
        *element.GetUInt16Pointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe ushort* GetUInt16Pointer<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return (ushort*)element.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int GetInt32<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return *element.GetInt32Pointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetInt32<TElement>(this TElement element, int index, int value)
        where TElement : IElement
    {
        *element.GetInt32Pointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe int* GetInt32Pointer<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return (int*)element.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe uint GetUInt32<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return *element.GetUInt32Pointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetUInt32<TElement>(this TElement element, int index, uint value)
        where TElement : IElement
    {
        *element.GetUInt32Pointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe uint* GetUInt32Pointer<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return (uint*)element.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe long GetInt64<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return *element.GetInt64Pointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetInt64<TElement>(this TElement element, int index, long value)
        where TElement : IElement
    {
        *element.GetInt64Pointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe long* GetInt64Pointer<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return (long*)element.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool GetBoolean<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return *element.GetBooleanPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetBoolean<TElement>(this TElement element, int index, bool value)
        where TElement : IElement
    {
        *element.GetBooleanPointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe bool* GetBooleanPointer<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return (bool*)element.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe byte GetByte<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return *element.GetBytePointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetByte<TElement>(this TElement element, int index, byte value)
        where TElement : IElement
    {
        *element.GetBytePointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe byte* GetBytePointer<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        return (byte*)element.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void* GetPointer<TElement>(this TElement element, int index)
        where TElement : IElement
    {
        var pool = element.GetPool();

        if (element.Properties.Length > 0 && element.Properties[0].Offset < 0)
        {
            var offset = 0;

            foreach (var property in element.Properties)
            {
                property.Offset = offset;

                offset += property.Size;
            }
        }

        var size = element.Properties[^1].Offset;

        return pool.GetPointer(element.EntityId, element.EntityIndex) + (element.ElementId - 1) * size + element.Properties[index].Offset;
    }
}