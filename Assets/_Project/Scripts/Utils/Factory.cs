using UnityEngine;
public abstract class Factory<T> : MonoBehaviour where T : MonoBehaviour
{
    protected Pool<T> pool = new();
    public abstract T Create(ObjectData<T> entityData, Vector3 position, Quaternion rotation);
}