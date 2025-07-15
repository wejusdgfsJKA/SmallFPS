using System.Collections.Generic;

public interface Identifiable<ID>
{
    ID GetID();
}
public class Pool<T>
{
    protected Stack<T> stack = new();
    /// <summary>
    /// Add an object to the pool.
    /// </summary>
    /// <param name="obj">The object to be added to the pool.</param>
    public void Release(T obj)
    {
        stack.Push(obj);
    }
    /// <summary>
    /// Get an object from the pool.
    /// </summary>
    /// <returns>An object, if any found, or the default value if none is found.</returns>
    public T Get()
    {
        if (stack.Count > 0)
        {
            return stack.Pop();
        }
        return default;
    }
}

