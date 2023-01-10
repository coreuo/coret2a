using System.Collections;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Core.Abstract.Domain;
using Core.Launcher.Domain;
using Core.Launcher.Extensions;

namespace Core.Launcher.Collections;

public readonly unsafe struct ConcurrentQueue<TEntity> : IProducerConsumerCollection<TEntity>
    where TEntity : IEntity
{
    internal int* Top { get; }

    internal int* Bottom => Top + 1;

    internal IPool<TEntity> Pool { get; }

    internal int Next { get; }

    public ConcurrentQueue(int* top, IPool<TEntity> pool, int next)
    {
        Top = top;
        Pool = pool;
        Next = next;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryDequeue(out TEntity entity)
    {
        do
        {
            var top = Volatile.Read(ref *Top);

            if (top == -1) continue;

            entity = Pool.GetValue(top);

            if (top == 0) return false;

            if (Interlocked.CompareExchange(ref *Top, entity.GetInt32(Next), top) != top) continue;

            entity.SetInt32(Next, 0);

            return true;

        } while (true);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Enqueue(TEntity entity)
    {
        do
        {
            var top = Volatile.Read(ref *Top);

            if (top == -1 || Interlocked.CompareExchange(ref *Top, -1, top) != top) continue;

            if (top == 0)
            {
                top = entity.Id;
            }
            else if (*Bottom > 0)
            {
                var existing = Pool.GetValue(*Bottom);

                existing.SetInt32(Next, entity.Id);
            }

            *Bottom = entity.Id;

            Volatile.Write(ref *Top, top);

            return;

        } while (true);
    }

    public IEnumerator<TEntity> GetEnumerator()
    {
        throw new NotSupportedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotSupportedException();
    }

    public void CopyTo(Array array, int index)
    {
        throw new NotSupportedException();
    }

    public int Count => throw new NotSupportedException();

    public bool IsSynchronized => true;

    public object SyncRoot => throw new NotSupportedException();

    public void CopyTo(TEntity[] array, int index)
    {
        throw new NotSupportedException();
    }

    public TEntity[] ToArray()
    {
        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryAdd(TEntity item)
    {
        Enqueue(item);

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryTake(out TEntity item)
    {
        return TryDequeue(out item);
    }
}