using UnityEngine;
using UnityEngine.Events;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField] UnityEvent<Transform> onEnter;
    [SerializeField] UnityEvent onEnterNoArgs;
    private void OnTriggerEnter(Collider other)
    {
        onEnter?.Invoke(other.transform.root);
        onEnterNoArgs?.Invoke();
    }
    private void OnDisable()
    {
        onEnter = null;
        onEnterNoArgs = null;
    }
}
