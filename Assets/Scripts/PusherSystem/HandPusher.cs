using PusherSystem.Configurations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PusherSystem
{
    public class HandPusher : MonoBehaviour
    {
        [SerializeField] private BallConfiguration _ballConfiguration;
        [SerializeField] private InputActionProperty _velocityProperty;

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.TryGetComponent<IPushable>(out var pushable))
                return;

            var velocity = GetVelocity();
            var direction = velocity.normalized;
            var force = direction * velocity.magnitude * _ballConfiguration.ForceMultiplier;

            pushable.Push(force, other.contacts[0].point);
        }

        private Vector3 GetVelocity() => _velocityProperty.action.ReadValue<Vector3>();
    }
}
