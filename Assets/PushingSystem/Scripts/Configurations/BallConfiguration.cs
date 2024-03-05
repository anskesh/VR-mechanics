using UnityEngine;

namespace PushingSystem.Configurations
{
    [CreateAssetMenu(fileName = "BallConfiguration", menuName = "Configurations/Ball")]
    public class BallConfiguration : ScriptableObject
    {
        public float ForceMultiplier = 30;
    }
}