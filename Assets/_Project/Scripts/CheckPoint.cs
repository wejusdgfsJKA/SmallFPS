using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public static GameObject ActiveCheckpoint { get; protected set; }
    private void OnEnable()
    {
        if (ActiveCheckpoint != gameObject)
        {
            ActiveCheckpoint.SetActive(false);
            ActiveCheckpoint = gameObject;
        }
    }
}
