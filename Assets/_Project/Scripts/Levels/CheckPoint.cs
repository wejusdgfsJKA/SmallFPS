using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{
    public static CheckPoint ActiveCheckpoint { get; protected set; }
    public UnityEvent OnRespawn;
    private void OnEnable()
    {
        if (ActiveCheckpoint != this)
        {
            ActiveCheckpoint?.gameObject.SetActive(false);
            ActiveCheckpoint = this;
        }
    }
}
