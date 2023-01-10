using System.Runtime.CompilerServices;
using Core.Abstract.Domain;
using Core.Launcher.Collections;
using Core.Launcher.Domain;

namespace Core.Launcher.Extensions;

public static class ObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue GetValue<TObject, TValue>(this TObject @object, IStore<TValue> store, int index)
        where TObject : IObject
    {
        var id = @object.GetInt32(index);

        return store.GetValue(id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetValue<TObject, TValue>(this TObject @object, IStore<TValue> store, int index, TValue value)
        where TObject : IObject
    {
        var id = store.GetId(value);

        @object.SetInt32(index, id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span<TValue> GetSpan<TObject, TValue>(this TObject @object, int index, int length)
        where TValue : struct
        where TObject : IObject
    {
        var pointer = @object.GetPointer(index);

        return new Span<TValue>(pointer, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ConcurrentQueue<TChild> GetConcurrentQueue<TParent, TChild>(this TParent parent, int parentIndex, IPool<TChild> children, int childrenIndex)
        where TParent : IEntity
        where TChild : IEntity
    {
        return new ConcurrentQueue<TChild>(parent.GetInt32Pointer(parentIndex), children, childrenIndex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Collections.List<TChild> GetList<TParent, TChild>(this TParent parent, int parentIndex, IPool<TChild> children, int childrenIndex)
        where TParent : IEntity
        where TChild : IEntity
    {
        return new Collections.List<TChild>(parent.Id, parent.GetInt32Pointer(parentIndex), children, childrenIndex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Array<TChild> GetArray<TParent, TChild>(this TParent parent, int index, int count)
        where TParent : IObject
        where TChild : IElement<TChild>
    {
        return new Array<TChild>(count, parent.GetPool(), parent.Id, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int ExchangeInt32<TObject>(this TObject @object, int index, int value)
        where TObject : IObject
    {
        var p = @object.GetInt32Pointer(index);

        var result = *p;

        *p = value;

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool GetFlag<TObject>(this TObject @object, int index, int flag)
        where TObject : IObject
    {
        return (*@object.GetBytePointer(index) & (1 << flag)) > 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetFlag<TObject>(this TObject @object, int index, int flag, bool value)
        where TObject : IObject
    {
        var p = @object.GetBytePointer(index);

        if (value) *p |= (byte)(1 << flag);

        else *p &= (byte)~(1 << flag);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe sbyte GetSByte<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return *@object.GetSBytePointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetSByte<TObject>(this TObject @object, int index, sbyte value)
        where TObject : IObject
    {
        *@object.GetSBytePointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe sbyte* GetSBytePointer<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return (sbyte*)@object.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ushort GetUInt16<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return *@object.GetUInt16Pointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetUInt16<TObject>(this TObject @object, int index, ushort value)
        where TObject : IObject
    {
        *@object.GetUInt16Pointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe ushort* GetUInt16Pointer<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return (ushort*)@object.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int GetInt32<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return *@object.GetInt32Pointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetInt32<TObject>(this TObject @object, int index, int value)
        where TObject : IObject
    {
        *@object.GetInt32Pointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe int* GetInt32Pointer<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return (int*)@object.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe uint GetUInt32<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return *@object.GetUInt32Pointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetUInt32<TObject>(this TObject @object, int index, uint value)
        where TObject : IObject
    {
        *@object.GetUInt32Pointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe uint* GetUInt32Pointer<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return (uint*)@object.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe long GetInt64<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return *@object.GetInt64Pointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetInt64<TObject>(this TObject @object, int index, long value)
        where TObject : IObject
    {
        *@object.GetInt64Pointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe long* GetInt64Pointer<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return (long*)@object.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool GetBoolean<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return *@object.GetBooleanPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetBoolean<TObject>(this TObject @object, int index, bool value)
        where TObject : IObject
    {
        *@object.GetBooleanPointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe bool* GetBooleanPointer<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return (bool*)@object.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe byte GetByte<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return *@object.GetBytePointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetByte<TObject>(this TObject @object, int index, byte value)
        where TObject : IObject
    {
        *@object.GetBytePointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe byte* GetBytePointer<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        return (byte*)@object.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void* GetPointer<TObject>(this TObject @object, int index)
        where TObject : IObject
    {
        var pool = @object.GetPool();

        return pool.Pointer + @object.GetOffset(index);
    }
}