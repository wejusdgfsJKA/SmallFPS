using UnityEngine;

/// <summary>
/// Damage package.
/// </summary>
public struct DmgInfo
{
    /// <summary>
    /// How much damage this attack has dealt.
    /// </summary>
    public int Damage { get; set; }
    public Transform Source { get; set; }
}
