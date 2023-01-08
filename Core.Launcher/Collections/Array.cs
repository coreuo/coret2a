using Core.Launcher.Domain;
using System.Collections;

namespace Core.Launcher.Collections
{
    public readonly struct Array<TElement> : IReadOnlyList<TElement>
        where TElement : IElement<TElement>
    {
        public int Count { get; }

        public Pool Pool { get; }

        public int Id { get; }

        public int Index { get; }

        public TElement this[int index] => TElement.Create(Pool, Id, Index, index + 1);

        public Array(int count, Pool pool, int id, int index)
        {
            Count = count;
            Pool = pool;
            Id = id;
            Index = index;
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
