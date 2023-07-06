using Catkey.StarSlayer.Core;
using UnityEngine;

namespace Catkey.StarSlayer.Managers
{
    public class SpawnManager : Utils.Singleton<SpawnManager>
    {
        [Header("Spawn Properties")]
        [SerializeField] float _initialSpawnMinDelay;
        [SerializeField] float _initialSpawnMaxDelay;
        [SerializeField] float _spawnMinDelayMinimumValue;
        [SerializeField] float _spawnMaxDelayMinimumValue;

        [Header("Spawn transforms")]
        [SerializeField] Transform _leftSpawnTransform;
        [SerializeField] Transform _centerSpawnTransform;
        [SerializeField] Transform _rightSpawnTransform;

        [Header("Prefabs")]
        [SerializeField] MeteoriteController _meteoritePrefab;

        private float _lastLeftSpawnTime;
        private float _lastCenterSpawnTime;
        private float _lastRightSpawnTime;

        private float _spawnMinDelay;
        private float _spawnMaxDelay;

        private void Start()
        {
            _lastLeftSpawnTime = 0.0f;
            _lastCenterSpawnTime = 0.0f;
            _lastRightSpawnTime = 0.0f;

            _spawnMinDelay = _initialSpawnMinDelay;
            _spawnMaxDelay = _initialSpawnMaxDelay;
        }

        private void Update()
        {
            if(GameManager.Instance.Gamestate == Utils.GameState.Playing || GameManager.Instance.Gamestate == Utils.GameState.GameOver) 
            {
                SpawnLeft();
                SpawnCenter();
                SpawnRight();
            }
        }

        public void ReduceSpawnDelay(float value) 
        {
            _spawnMinDelay -= value;
            _spawnMaxDelay -= value;

            _spawnMinDelay = Mathf.Clamp(_spawnMinDelay, _spawnMinDelayMinimumValue, _initialSpawnMinDelay);
            _spawnMaxDelay = Mathf.Clamp(_spawnMaxDelay, _spawnMaxDelayMinimumValue, _initialSpawnMaxDelay);
        }

        private void SpawnLeft()
        {
            if (_lastLeftSpawnTime < Time.time)
            {
                _lastLeftSpawnTime = Time.time + Random.Range(_spawnMinDelay, _spawnMaxDelay);
                SpawnColorCircle(_leftSpawnTransform.position);
            }
        }

        private void SpawnCenter() 
        {
            if (_lastCenterSpawnTime < Time.time)
            {
                _lastCenterSpawnTime = Time.time + Random.Range(_spawnMinDelay, _spawnMaxDelay);
                SpawnColorCircle(_centerSpawnTransform.position);
            }
        }

        private void SpawnRight()
        {
            if (_lastRightSpawnTime < Time.time)
            {
                _lastRightSpawnTime = Time.time + Random.Range(_spawnMinDelay, _spawnMaxDelay);
                SpawnColorCircle(_rightSpawnTransform.position);
            }
        }

        private void SpawnColorCircle(Vector3 spawnPosition)
        {
            Instantiate(_meteoritePrefab).InitializeMeteorite(spawnPosition);
        }
    }
}

