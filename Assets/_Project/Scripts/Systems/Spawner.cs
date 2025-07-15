using KBCore.Refs;
using UnityEngine;

public class Spawner : ValidatedMonoBehaviour
{
    [SerializeField, Anywhere] EntityType entity;
    private void OnEnable()
    {

    }
}
