using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PushableSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Pushable _template;
    [SerializeField] private float _lifeTime = 10;
    [SerializeField] private int _countOnStart = 5;
    
    private List<Pushable> _pushables = new List<Pushable>();
    private bool _isSpawning = false;
    
    private void Awake()
    {
        for (var i = 0; i < _countOnStart; i++)
            _pushables.Add(Create());
    }

    private void Start()
    {
        Get();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Pushable pushable) && _isSpawning == false)
            StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        _isSpawning = true;
        yield return new WaitForSeconds(2);
        Get();
        _isSpawning = false;
    }

    private Pushable Get()
    {
        Pushable pushable = null;

        if (_pushables.Any(x => x.gameObject.activeSelf == false))
            pushable = _pushables.First(x => x.gameObject.activeSelf == false);
        else
            pushable = Create();
        
        pushable.transform.localPosition = Vector3.zero;
        pushable.transform.rotation = Quaternion.identity;
        pushable.gameObject.SetActive(true);
        return pushable;
    }

    private Pushable Create()
    {
        var pushable = Instantiate(_template, _spawnPoint);
        pushable.gameObject.SetActive(false);
        pushable.Init(_lifeTime);
        _pushables.Add(pushable);

        return pushable;
    }
}
