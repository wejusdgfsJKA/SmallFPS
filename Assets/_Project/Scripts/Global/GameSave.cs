using System;

[Serializable]
public class GameData
{
    public int NextLevel;
    public GameData(int currentLevel)
    {
        this.NextLevel = currentLevel;
    }
}
