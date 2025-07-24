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
        /// <summary>
        /// Get a valid spawn point.
        /// </summary>
        /// <returns>A transform where spawning is valid.</returns>
        Transform GetSpawnPoint();
    }
    /// <summary>
    /// Picks spawn points in order, loops around to the start on reaching the end of the list.
    /// </summary>
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