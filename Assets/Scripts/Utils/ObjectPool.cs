using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class ObjectPool<T>
{
    private class ObjectPoolEmptyException : ApplicationException
    {
    }

    private ConcurrentBag<T> bag;

    public ObjectPool() => this.bag = new ConcurrentBag<T>();

    public T Take()
    {
        if (!bag.TryTake(out T result))
            throw new ObjectPoolEmptyException();

        return result;
    }

    public void Put(T objectToPut) => bag.Add(objectToPut);
}
