using EventBus;

namespace Levels
{
    /// <summary>
    /// Fires when the player reaches a checkpoint.
    /// </summary>
    public struct CheckpointReached : IEvent { }
    /// <summary>
    /// Fires when the player dies.
    /// </summary>
    public struct PlayerDeath : IEvent { }
}