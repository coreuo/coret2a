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

    public static abstract int GetSize();

    public static abstract int GetPoolCapacity();
}

public interface IEntity : IObject
{
    public int Free { get; set; }
}