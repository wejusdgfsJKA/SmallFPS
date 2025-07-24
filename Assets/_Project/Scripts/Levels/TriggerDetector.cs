using UnityEngine;
using UnityEngine.Events;

public class TriggerDetector : MonoBehaviour
{
    /// <summary>
    /// Fires when something enters this trigger.
    /// </summary>
    [SerializeField] UnityEvent onEnterNoArgs;
    private void OnTriggerEnter(Collider other)
    {
        onEnterNoArgs?.Invoke();
    }
}
