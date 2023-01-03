﻿using System.Collections;
using Core.Abstract.Domain;
using Core.Launcher.Domain;
using Core.Launcher.Extensions;

namespace Core.Launcher.Collections;

public struct Enumerator<TEntity> : IEnumerator<TEntity>
    where TEntity : IEntity
{
    private readonly IPool<TEntity> _pool;

    private readonly int _initial;

    private readonly int _next;

    private int _current = -1;

    private int _future = -1;

    public TEntity Current => _pool.GetValue(_current);

    object IEnumerator.Current => Current;

    public Enumerator(IPool<TEntity> pool, int initial, int next)
    {
        _pool = pool;
        _next = next;
        _initial = initial;
        _future = initial;
    }

    public bool MoveNext()
    {
        _current = _future;

        if (_current > 0)
        {
            _future = Current.GetInt32(_next);

            return true;
        }

        return false;
    }

    public void Reset()
    {
        _current = -1;

        _future = _initial;
    }

    public void Dispose()
    {
    }
}