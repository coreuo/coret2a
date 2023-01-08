using System.Collections;
using System.Runtime.CompilerServices;

namespace Core.Launcher.Domain;

public interface IEntity<TPool, TEntity> : IEntity<TEntity>
    where TEntity : IEntity<TPool, TEntity>
{
    public TPool Pool { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static abstract TEntity Create(TPool pool, int id);
}

public interface IEntity<TEntity> : IEntity
    where TEntity : IEntity<TEntity>
{
    public static abstract Property[] GetProperties();

    public static abstract int GetPoolCapacity();

    public readonly struct Array<TElement> : IReadOnlyList<TElement>
        where TElement : IElement<TEntity, TElement>
    {
        private readonly TEntity _entity;

        private readonly int _index;

        public int Count { get; }

        public TElement this[int index] => TElement.Create(_entity, _index, index + 1);

        public Array(TEntity entity, int index, int count)
        {
            _entity = entity;
            _index = index;
            Count = count;
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<TElement>
        {
            private readonly Array<TElement> _array;

            private int _next = -1;

            public Enumerator(Array<TElement> array)
            {
                _array = array;
            }

            public bool MoveNext()
            {
                _next++;

                return _next < _array.Count;
            }

            public void Reset()
            {
                _next = -1;
            }

            public TElement Current => _array[_next];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}

public interface IEntity
{
    int Id { get; }

    public int Free { get; set; }

    public Pool GetPool();
}