using System.Runtime.CompilerServices;
using Core.Abstract.Domain;
using Core.Launcher.Collections;
using Core.Launcher.Domain;

namespace Core.Launcher.Extensions;

public static class EntityExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue GetValue<TEntity, TValue>(this TEntity entity, IStore<TValue> store, int index)
        where TEntity : IEntity
    {
        var id = entity.GetInt32(index);

        return store.GetValue(id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetValue<TEntity, TValue>(this TEntity entity, IStore<TValue> store, int index, TValue value)
        where TEntity : IEntity
    {
        var id = store.GetId(value);

        entity.SetInt32(index, id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span<TValue> GetSpan<TEntity, TValue>(this TEntity entity, int index, int length)
        where TValue : struct
        where TEntity : IEntity
    {
        var pointer = entity.GetPointer(index);

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
    public static unsafe int ExchangeInt32<TEntity>(this TEntity entity, int index, int value)
        where TEntity : IEntity
    {
        var p = entity.GetInt32Pointer(index);

        var result = *p;

        *p = value;

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int GetInt32<TEntity>(this TEntity entity, int index)
        where TEntity : IEntity
    {
        return *entity.GetInt32Pointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetInt32<TEntity>(this TEntity entity, int index, int value)
        where TEntity : IEntity
    {
        *entity.GetInt32Pointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe int* GetInt32Pointer<TEntity>(this TEntity entity, int index)
        where TEntity : IEntity
    {
        return (int*)entity.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe uint GetUInt32<TEntity>(this TEntity entity, int index)
        where TEntity : IEntity
    {
        return *entity.GetUInt32Pointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetUInt32<TEntity>(this TEntity entity, int index, uint value)
        where TEntity : IEntity
    {
        *entity.GetUInt32Pointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe uint* GetUInt32Pointer<TEntity>(this TEntity entity, int index)
        where TEntity : IEntity
    {
        return (uint*)entity.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe long GetInt64<TEntity>(this TEntity entity, int index)
        where TEntity : IEntity
    {
        return *entity.GetInt64Pointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetInt64<TEntity>(this TEntity entity, int index, long value)
        where TEntity : IEntity
    {
        *entity.GetInt64Pointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe long* GetInt64Pointer<TEntity>(this TEntity entity, int index)
        where TEntity : IEntity
    {
        return (long*)entity.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool GetBoolean<TEntity>(this TEntity entity, int index)
        where TEntity : IEntity
    {
        return *entity.GetBooleanPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetBoolean<TEntity>(this TEntity entity, int index, bool value)
        where TEntity : IEntity
    {
        *entity.GetBooleanPointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe bool* GetBooleanPointer<TEntity>(this TEntity entity, int index)
        where TEntity : IEntity
    {
        return (bool*)entity.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe byte GetByte<TEntity>(this TEntity entity, int index)
        where TEntity : IEntity
    {
        return *entity.GetBytePointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetByte<TEntity>(this TEntity entity, int index, byte value)
        where TEntity : IEntity
    {
        *entity.GetBytePointer(index) = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe byte* GetBytePointer<TEntity>(this TEntity entity, int index)
        where TEntity : IEntity
    {
        return (byte*)entity.GetPointer(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void* GetPointer<TEntity>(this TEntity entity, int index)
        where TEntity : IEntity
    {
        var pool = entity.GetPool();

        return pool.GetPointer(entity.Id, index);
    }
}