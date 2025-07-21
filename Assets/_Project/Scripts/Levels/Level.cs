using System.Collections.Generic;
using UnityEngine;
namespace Levels
{
    public class Level : MonoBehaviour
    {
        public static Level CurrentLevel { get; private set; }
        protected HashSet<Encounter> encounters = new();
        private void Awake()
        {
            CurrentLevel = this;
        }
        private void Start()
        {
            GameManager.Instance.SpawnPlayer();
        }
        public void LevelFinished()
        {
            CurrentLevel = null;
            GameManager.Instance.NextLevel();
        }
    }
}