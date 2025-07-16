using System.Collections.Generic;
using UnityEngine;
public enum SpawnStrategyType
{
    Linear
}
public interface ISpawnStrategy
{
    Transform GetSpawnPoint();
}
public class LinearSpawnStrategy : ISpawnStrategy
{
    int index = -1;
    protected List<Transform> points;
    public LinearSpawnStrategy(List<Transform> points)
    {
        this.points = points;
    }
    public Transform GetSpawnPoint()
    {
        index++;
        return points[index];
    }
}

