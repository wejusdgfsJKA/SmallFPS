using MBT;
using UnityEngine;
using Weapon;

[AddComponentMenu("")]
// Register node in visual editor node finder
[MBTNode(name = "Tasks/ShootTask")]
public class ShootTask : Leaf
{
    [SerializeField] WeaponBase weapon;
    [SerializeField] TransformReference player;
    public override void OnEnter()
    {
        base.OnEnter();
        weapon.StartFiring();
    }
    public override NodeResult Execute()
    {
        if (player == null)
        {
            return NodeResult.failure;
        }
        weapon.transform.LookAt(player.Value.position);
        if (!weapon.Firing)
        {
            weapon.StartFiring();
        }
        return NodeResult.running;
    }
    public override void OnExit()
    {
        base.OnExit();
        weapon.StopFiring();
    }
}
