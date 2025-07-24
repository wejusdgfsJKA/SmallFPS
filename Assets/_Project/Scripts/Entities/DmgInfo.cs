using EventBus;
using UnityEngine;

/// <summary>
/// Damage container.
/// </summary>
public struct DmgInfo : IEvent
{
    /// <summary>
    /// How much damage this attack has dealt.
    /// </summary>
    public int Damage { get; set; }
    /// <summary>
    /// The entity responsible for dealing the damage, should be null for environmental hazards.
    /// </summary>
    public Transform Source { get; set; }
    public DmgInfo(int damage, Transform source = null)
    {
        Damage = damage;
        Source = source;
    }
}
