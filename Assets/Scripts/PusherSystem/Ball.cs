using System.Collections;
using UnityEngine;

namespace PusherSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour, IPushable
    {
        private float _lifeTime;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Init(float lifeTime)
        {
            _lifeTime = lifeTime;
        }

        public void Push(Vector3 force, Vector3 collisionPoint)
        {
            Debug.Log(force);
            _rigidbody.AddForceAtPosition(force, collisionPoint, ForceMode.Impulse);
            StartCoroutine(Disable());
        }

        private IEnumerator Disable()
        {
            yield return new WaitForSeconds(_lifeTime);
            gameObject.SetActive(false);
            ResetVelocity();
        }
        
        private void ResetVelocity()
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
}
