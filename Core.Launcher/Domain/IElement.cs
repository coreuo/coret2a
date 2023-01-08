using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Core.Launcher.Domain
{
    public interface IElement<TElement> : IElement
        where TElement : IElement<TElement>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static abstract TElement Create(Pool pool, int entityId, int entityIndex, int elementId);
    }

    public interface IElement
    {
        Pool Pool { get; }

        int EntityId { get; }

        int EntityIndex { get; }

        int Id { get; }

        Property[] Properties { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static abstract int GetSize();
    }
}
