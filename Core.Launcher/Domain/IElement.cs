using System.Runtime.CompilerServices;

namespace Core.Launcher.Domain
{
    public interface IElement<TEntity, TElement> : IElement
        where TEntity : IEntity<TEntity>
        where TElement : IElement<TEntity, TElement>
    {
        TEntity Entity { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static abstract TElement Create(TEntity entity, int index, int id);
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
