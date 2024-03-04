using System;
using System.Collections;
using UnityEngine;

namespace PusherSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour, IPushable
    {
        private Rigidbody _rigidbody;
        private bool _canDisable;
        private float _lifeTime;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Initialize(float lifeTime)
        {
            _lifeTime = lifeTime;
        }

        private void OnEnable()
        {
            _rigidbody.isKinematic = false;
        }

        private void Update()
        {
            if (_canDisable == false)
                return;
            
            Disable();
        }

        public void Push()
        {
            StartCoroutine(WaitDisable());
        }

        public void Push(Vector3 force, Vector3 collisionPoint)
        {
            _rigidbody.AddForceAtPosition(force, collisionPoint, ForceMode.Impulse);
            Push();
        }

        private IEnumerator WaitDisable()
        {
            yield return new WaitForSeconds(_lifeTime);
            _canDisable = true;
        }

        private void Disable()
        {
            _canDisable = false;
            gameObject.SetActive(false);
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(0,0,0);
        }
    }
}
