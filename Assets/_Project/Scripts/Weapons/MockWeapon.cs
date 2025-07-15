using UnityEngine;

public class MockWeapon : Weapon
{
    protected override void Awake()
    {
        base.Awake();
        onFire += () =>
        {
            Debug.Log("Gay");
        };
        onAltFire += () =>
        {
            Debug.Log("Straight");
        };
    }
}
