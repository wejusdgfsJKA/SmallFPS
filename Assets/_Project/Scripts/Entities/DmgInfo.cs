using EventBus;
using UnityEngine;

/// <summary>
/// Damage package.
/// </summary>
public struct DmgInfo : IEvent
{
    /// <summary>
    /// How much damage this attack has dealt.
    /// </summary>
    public int Damage { get; set; }
    public Transform Source { get; set; }
    public DmgInfo(int damage, Transform source)
    {
        Damage = damage;
        Source = source;
    }
}
