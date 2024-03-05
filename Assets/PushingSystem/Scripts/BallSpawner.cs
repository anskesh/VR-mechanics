using System.Collections;
using System.Collections.Generic;
using PushingSystem.Configurations;
using UnityEngine;

namespace PushingSystem
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private SpawnerConfiguration _spawnerConfiguration;
        [SerializeField] private Transform _spawnPoint;

        private Queue<Ball> _balls;
        private bool _isSpawning;

        private void Awake()
        {
            _balls = new Queue<Ball>(_spawnerConfiguration.CountOnStart);

            for (int i = 0; i < _spawnerConfiguration.CountOnStart; i++)
                Create();
        }

        private void Start()
        {
            GetOrCreate();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!_isSpawning && other.CompareTag("Ball"))
                StartCoroutine(Spawn());
        }

        private void OnDestroy()
        {
            foreach (Ball ball in _balls)
                ball.DisabledBallEvent -= OnBallDisabled;
        }

        private IEnumerator Spawn()
        {
            _isSpawning = true;
            yield return new WaitForSeconds(_spawnerConfiguration.RespawnDelay);
            GetOrCreate();
            _isSpawning = false;
        }

        private void Create()
        {
            Ball ball = Instantiate(_spawnerConfiguration.Template, _spawnPoint);
            ball.gameObject.SetActive(false);
            ball.Initialize(_spawnerConfiguration.LifeTime);
            ball.DisabledBallEvent += OnBallDisabled;
            
            Add(ball);
        }

        private void Add(Ball ball)
        {
            _balls.Enqueue(ball);
        }

        private void GetOrCreate()
        {
            if (_balls.Count == 0)
                Create();

            Ball ball = _balls.Dequeue();
            ball.transform.localPosition = Vector3.zero;
            ball.transform.rotation = Quaternion.identity;
            ball.gameObject.SetActive(true);
        }

        private void OnBallDisabled(Ball ball)
        {
            Add(ball);
        }
    }
}