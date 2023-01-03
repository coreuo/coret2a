using System.Collections;
using System.Runtime.CompilerServices;
using Core.Abstract.Domain;
using Core.Launcher.Domain;
using Core.Launcher.Extensions;

namespace Core.Launcher.Collections;

public readonly unsafe struct List<TChild> : ICollection<TChild>
    where TChild : IEntity
{
    private readonly int _parentId;

    private readonly int* _count;

    public int Count
    {
        get => *_count;
        set => *_count = value;
    }

    public bool IsReadOnly => false;

    internal int* Top => _count + 1;

    internal int* Bottom => Top + 1;

    internal IPool<TChild> Pool { get; }

    internal int Owner { get; }

    internal int Next => Owner + 1;

    internal int Previous => Next + 1;

    internal List(int parentId, int *count, IPool<TChild> children, int childrenIndex)
    {
        _parentId = parentId;
        _count = count;
        Pool = children;
        Owner = childrenIndex;
    }

    public IEnumerator<TChild> GetEnumerator()
    {
        return new Enumerator<TChild>(Pool, *Top, Next);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(TChild entity)
    {
        if (*Top == 0)
        {
            *Top = entity.Id;
        }
        else if (*Bottom > 0)
        {
            var existing = Pool.GetValue(*Bottom);

            existing.SetInt32(Next, entity.Id);

            entity.SetInt32(Previous, existing.Id);
        }

        *Bottom = entity.Id;

        entity.SetInt32(Owner, _parentId);

        Count++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
        foreach (var child in this)
        {
            Remove(child);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(TChild child)
    {
        return _parentId == child.GetInt32(Owner);
    }

    public void CopyTo(TChild[] array, int arrayIndex)
    {
        var i = 0;

        foreach (var child in this)
        {
            array[arrayIndex + i] = child;

            i++;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Remove(TChild child)
    {
        if (!Contains(child)) return false;

        var previous = ExchangePrevious(child, 0);

        var next = ExchangeNext(child, 0);

        if (previous.Id == 0) *Top = 0;

        else if(next.Id > 0) previous.SetInt32(Next, next.Id);

        else previous.SetInt32(Next, 0);

        if (next.Id == 0) *Bottom = 0;

        else if(previous.Id > 0) next.SetInt32(Previous, previous.Id);

        else next.SetInt32(Previous, 0);

        child.SetInt32(Owner, 0);

        Count--;

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TChild ExchangePrevious(TChild child, int value)
    {
        return Pool.GetValue(child.ExchangeInt32(Previous, value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TChild ExchangeNext(TChild child, int value)
    {
        return Pool.GetValue(child.ExchangeInt32(Next, value));
    }

    /*public int IndexOf(TChild item)
    {
        var i = 0;

        foreach (var child in this)
        {
            if (child.Id == item.Id) return i;

            i++;
        }

        return -1;
    }

    public void Insert(int index, TChild child)
    {
        var current = this[index];

        var previous = ExchangePrevious(current, child.Id);

        previous.SetInt32(Next, child.Id);

        child.SetInt32(Previous, previous.Id);

        child.SetInt32(Next, current.Id);
    }

    public void RemoveAt(int index)
    {
        var child = this[index];

        Remove(child);
    }

    public TChild this[int index]
    {
        get
        {
            var i = 0;

            foreach (var child in this)
            {
                if(i == index) return child;

                i++;
            }

            throw new IndexOutOfRangeException();
        }
        set
        {
            Remove(this[index]);

            Insert(index, value);
        }
    }*/
}