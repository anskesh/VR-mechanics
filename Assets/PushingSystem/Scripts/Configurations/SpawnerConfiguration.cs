using UnityEngine;

namespace PushingSystem.Configurations
{
    [CreateAssetMenu(fileName = "SpawnerConfiguration", menuName = "Configurations/Spawner")]
    public class SpawnerConfiguration : ScriptableObject
    {
        public Ball Template;
        public float LifeTime = 8;
        public int CountOnStart = 5;
        public float RespawnDelay = 0.6f;
    }
}