using System;

/// <summary>
/// Stores data about the player's progress.
/// </summary>
[Serializable]
public struct GameData
{
    /// <summary>
    /// The level that should be loaded when the player presses 'continue' in the main menu.
    /// </summary>
    public int NextLevel;
    public GameData(int currentLevel)
    {
        NextLevel = currentLevel;
    }
}
