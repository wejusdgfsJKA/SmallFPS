using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private void OnEnable()
    {
        transform.GetComponent<Interactable>()?.AddAction(EndLevel);
    }
    private void OnDisable()
    {
        transform.GetComponent<Interactable>()?.RemoveAction(EndLevel);
    }
    public void EndLevel(Transform player)
    {
        Level.CurrentLevel.LevelFinished();
    }
}
