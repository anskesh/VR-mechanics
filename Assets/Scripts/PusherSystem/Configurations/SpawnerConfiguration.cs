using UnityEngine;

namespace PusherSystem.Configurations
{
    [CreateAssetMenu(fileName = "SpawnerConfiguration", menuName = "Configurations/Spawner")]
    public class SpawnerConfiguration : ScriptableObject
    {
        [SerializeField] public Ball Template;
        [SerializeField] public float LifeTime = 10;
        [SerializeField] public int CountOnStart = 5;
        [SerializeField] public float RespawnDelay = 1;
    }
}