using System.Runtime.CompilerServices;

namespace Core.Launcher.Domain;

public interface IElement<TElement> : IElement
    where TElement : IElement<TElement>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static abstract TElement Create(Pool pool, int elementId, int entityId, int entityIndex);
}

public interface IElement : IObject
{
    int EntityId { get; }

    int EntityIndex { get; }
}