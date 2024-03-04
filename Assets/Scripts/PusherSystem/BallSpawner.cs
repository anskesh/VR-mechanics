using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PusherSystem.Configurations;
using UnityEngine;

namespace PusherSystem
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private SpawnerConfiguration _spawnerConfiguration;

        private List<Ball> _balls = new List<Ball>();
        private bool _isSpawning = false;

        private void Awake()
        {
            for (var i = 0; i < _spawnerConfiguration.CountOnStart; i++)
                _balls.Add(Create());
        }

        private void Start()
        {
            GetOrCreate();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Ball ball) && _isSpawning == false)
                StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            _isSpawning = true;
            yield return new WaitForSeconds(_spawnerConfiguration.RespawnDelay);
            GetOrCreate();
            _isSpawning = false;
        }

        private void GetOrCreate()
        {
            if (!Get(out var ball))
                ball = Create();

            ball.transform.localPosition = Vector3.zero;
            ball.transform.rotation = Quaternion.identity;
            ball.gameObject.SetActive(true);
        }

        private bool Get(out Ball ball)
        {
            ball = null;

            if (_balls.All(x => x.gameObject.activeSelf))
                return false;

            ball = _balls.First(x => x.gameObject.activeSelf == false);
            return true;
        }

        private Ball Create()
        {
            var pushable = Instantiate(_spawnerConfiguration.Template, _spawnPoint);
            pushable.gameObject.SetActive(false);
            pushable.Init(_spawnerConfiguration.LifeTime);
            _balls.Add(pushable);

            return pushable;
        }
    }

}