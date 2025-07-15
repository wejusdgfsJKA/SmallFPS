using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponData")]
[System.Serializable]
public class WeaponData : ScriptableObject
{
    public Weapon.Type Type;
    public int MaxAmmo;
    public float AmmoRecharge;
    public float Cooldown;
    public float FireCost;
    public float AltFireCost;
}
