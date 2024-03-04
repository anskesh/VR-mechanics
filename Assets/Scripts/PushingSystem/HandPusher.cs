using PushingSystem.Configurations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PushingSystem
{
    public class HandPusher : MonoBehaviour
    {
        [SerializeField] private BallConfiguration _ballConfiguration;
        [SerializeField] private InputActionProperty _velocityProperty;

        private void Awake()
        {
            _velocityProperty.action.Enable();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.TryGetComponent<IPushable>(out var pushable))
                return;

            var velocity = GetVelocity();
            
            if (velocity == Vector3.zero)
            {
                pushable.Push();
                return;
            }

            var contactPoint = other.contacts[0].point;
            var direction = (contactPoint - transform.position).normalized;
            var force = direction * velocity.magnitude * _ballConfiguration.ForceMultiplier;

            pushable.Push(force, contactPoint);
        }

        private Vector3 GetVelocity() => _velocityProperty.action.ReadValue<Vector3>();
    }
}
