using System.Runtime.CompilerServices;
using Core.Launcher.Collections;

namespace Core.Launcher.Domain
{
    public interface IElement<TElement> : IElement
        where TElement : IElement<TElement>
    {
        public Array<TElement> Array { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static abstract TElement Create(Array<TElement> array, int id);
    }

    public interface IElement
    {
        int EntityId { get; }

        int EntityIndex { get; }

        int ElementId { get; }

        public Property[] Properties { get; }

        public Pool GetPool();
    }
}
