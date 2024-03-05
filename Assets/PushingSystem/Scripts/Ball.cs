using System;
using System.Collections;
using UnityEngine;

namespace PushingSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        public Action<Ball> DisabledBallEvent;

        private bool _isWaitingDisable;
        private Rigidbody _rigidbody;
        private float _lifeTime;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _rigidbody.isKinematic = false;
        }

        public void Initialize(float lifeTime)
        {
            _lifeTime = lifeTime;
        }

        public void Push(Vector3 force, Vector3 collisionPoint)
        {
            _rigidbody.AddForceAtPosition(force, collisionPoint, ForceMode.Impulse);
            
            if (!_isWaitingDisable)
                StartCoroutine(Disable());
        }

        private IEnumerator Disable()
        {
            _isWaitingDisable = true;
            yield return new WaitForSeconds(_lifeTime);
            
            gameObject.SetActive(false);
            DisabledBallEvent?.Invoke(this);
            
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
            _isWaitingDisable = false;
        }
    }
}