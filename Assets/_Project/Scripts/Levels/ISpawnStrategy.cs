using System.Collections.Generic;
using UnityEngine;
namespace Levels
{
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
            index = (index + 1) % points.Count;
            return points[index];
        }
    }
}