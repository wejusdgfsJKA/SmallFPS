using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    /// <summary>
    /// This fires when this object is interacted with.
    /// </summary>
    public UnityEvent<Transform> OnInteract;
    protected void OnEnable()
    {
        InteractableManager.Instance.Register(this);
    }
    protected void OnDisable()
    {
        OnInteract = null;
        InteractableManager.Instance.DeRegister(transform);
    }
    protected void OnDestroy()
    {
        OnDisable();
    }
    /// <summary>
    /// Interact with this object.
    /// </summary>
    /// <param name="source">The entity that interacted with this object.</param>
    public void Interact(Transform source)
    {
        OnInteract.Invoke(source);
    }
}