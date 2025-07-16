using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level CurrentLevel { get; private set; }
    protected HashSet<Encounter> encounters = new();
    private void OnEnable()
    {
        CurrentLevel = this;
        GameManager.Instance.RespawnPlayer();
    }
    public void RegisterEncounter(Encounter encounter)
    {
        encounters.Add(encounter);
    }
    public void CheckpointReached()
    {
        //clear completed encounters
        encounters.RemoveWhere(e => e.Completed);
    }
    public void ResetEncounters()
    {
        foreach (var item in encounters)
        {
            item.ResetEncounter();
        }
    }
    public void LevelFinished()
    {
        GameManager.Instance.NextLevel();
    }
}
