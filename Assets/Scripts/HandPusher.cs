using UnityEngine;
using UnityEngine.InputSystem;

public class HandPusher : MonoBehaviour
{
    [SerializeField] private BallConfiguration _ballConfiguration;
    [SerializeField] private InputActionProperty _velocityProperty;

    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.TryGetComponent<Pushable>(out var pushable))
            return;

        var velocity = GetVelocity();
        
        if (velocity == Vector3.zero)  
            return;
        
        //var direction = (other.contacts[0].point - transform.position).normalized;
        var force = velocity * _ballConfiguration.ForceMultiplier;
        
        Debug.Log(force);
        pushable.Push(force, other.contacts[0].point);
    }

    private Vector3 GetVelocity() => _velocityProperty.action.ReadValue<Vector3>();
}
