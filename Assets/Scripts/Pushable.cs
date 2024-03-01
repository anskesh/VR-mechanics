using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
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
    
    public void Push(Vector3 collisionPoint, Vector3 force)
    {
        Debug.Log("Push");
        _rigidbody.AddForceAtPosition(force, collisionPoint, ForceMode.Impulse);
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }
}
