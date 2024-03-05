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
            if (!other.collider.TryGetComponent(out Ball pushable))
                return;

            Vector3 velocity = GetVelocity();

            if (velocity == Vector3.zero)
            {
                pushable.Push(velocity, velocity);
                return;
            }

            Vector3 contactPoint = other.contacts[0].point;
            Vector3 direction = (contactPoint - transform.position).normalized;
            Vector3 force = direction * velocity.magnitude * _ballConfiguration.ForceMultiplier;

            pushable.Push(force, contactPoint);
        }

        private Vector3 GetVelocity() { return _velocityProperty.action.ReadValue<Vector3>(); }
    }
}