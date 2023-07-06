using Catkey.StarSlayer.Managers;
using Catkey.StarSlayer.Utils;
using UnityEngine;

namespace Catkey.StarSlayer.Core 
{
    public class MeteoriteController : MonoBehaviour, IDamageable
    {
        [Header("Components")]
        [SerializeField] private Collider _collider;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeteoriteEffects _effectsController;
        [SerializeField] private Transform _meteoriteTransform;
        [SerializeField] private RotateGameObject _meteoriteRotationController;
        [SerializeField] private Outline _outline;

        [Header("Materials")]
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _crownMaterial;
        [SerializeField] private Material _bigMaterial;
        [SerializeField] private Material _undestructibleMaterial;

        [Header("Meshes")]
        [SerializeField] private Mesh[] _meteoriteMeshes;
        [SerializeField] private Mesh _crownMesh;
        [SerializeField] private Mesh _bigMesh;
        [SerializeField] private Mesh _undestructibleMesh;

        [Header("Attributes")]
        [SerializeField] private float _lifes;
        [SerializeField] private float _speed;
        [SerializeField] private int _scoreToAdd;

        [Header("Level attributes")]
        [SerializeField] private int _firstLevel;
        [SerializeField] private int _secondLevel;
        [SerializeField] private int _thirdLevel;
        [SerializeField] private int _fourthLevel;
        [SerializeField] private int _fifthLevel;
        [SerializeField] private int _sixLevel;
        [SerializeField] private int _eightLevel;

        private TMeteoriteType _type;
        private TPerkType _perkType;
        private Vector3 _direction;
        private bool _isActive;

        #region Unity events

        private void Awake()
        {
            _isActive = false;
            _direction = Vector3.back;
        }

        private void FixedUpdate()
        {
            if (!_isActive)
                return;
            Move();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player" && GameManager.Instance.Gamestate == GameState.Playing)
            {
                if(_type != TMeteoriteType.Crown)
                    GameManager.Instance.SubstractPlayerLife();
                if(_type == TMeteoriteType.Crown)
                    GameManager.Instance.AddCrown(1);
                DestroyMeteorite();
            }
        }
        #endregion

        #region Public methods

        /// <summary>
        /// This function is called by the SpawnManager which contains the spawn position and control the spawn system
        /// </summary>
        /// <param name="position">New circle position</param>
        public void InitializeMeteorite(Vector3 position)
        {
            RandomizeMeteoriteType();

            transform.position = position;

            _speed += GameManager.Instance.SpeedBoost;
            _lifes += GameManager.Instance.LifeBoost;

            _isActive = true;

            _effectsController.PlayStartingMeteoriteAnimation();
        }

        /// <summary>
        /// Interface implementation IDamageable. Reduce _lifes from impacted meteorite.
        /// </summary>
        /// <param name="damage"></param>
        public void GetDamage(float damage)
        {
            if (_type == TMeteoriteType.Crown || _type == TMeteoriteType.Undestructible)
                return;

            _effectsController.PlayCollisionEffects();

            _lifes -= damage;

            if (_lifes <= 0)
            {
                if(_type != TMeteoriteType.Crown)
                    GameManager.Instance.AddScore(_scoreToAdd);

                DestroyMeteorite();
            }
        }
        #endregion

        #region Private methods
        private void Move()
        {
            transform.position += _direction * _speed * Time.fixedDeltaTime;
        }

        private void RandomizeMeteoriteType()
        {
            if (GameManager.Instance.GetDifficulty() < _firstLevel)
                _type = TMeteoriteType.Default;
            else if(GameManager.Instance.GetDifficulty() < _secondLevel)
            {
                SetMediumDifficultyType(2, -10);
            }
            else  if(GameManager.Instance.GetDifficulty() < _thirdLevel)
            {
                SetMediumDifficultyType(2, 1);
            }
            else if(GameManager.Instance.GetDifficulty() < _fourthLevel) 
            {
                SetHardDifficultyType(1, 0, 2);
            }
            else if (GameManager.Instance.GetDifficulty() < _fifthLevel) 
            {
                SetHardDifficultyType(1, 30, 2);
            }
            else if (GameManager.Instance.GetDifficulty() < _sixLevel)
            {
                SetHardDifficultyType(1, 50, 2);
            }
            else if(GameManager.Instance.GetDifficulty() < _eightLevel) 
            {
                SetHardDifficultyType(1, 70, 2);
            }


            switch (_type) 
            {
                case TMeteoriteType.Default:
                    _renderer.material = _defaultMaterial;
                    break;
                case TMeteoriteType.Crown:
                    _meteoriteRotationController.InitializeRotation(0, 15, 0);
                    _meshFilter.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    _meshFilter.mesh = _crownMesh;
                    _renderer.material = _crownMaterial;
                    break;
                case TMeteoriteType.Big:
                    _meshFilter.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                    _meshFilter.mesh = _bigMesh;
                    _renderer.material = _bigMaterial;
                    break;
                case TMeteoriteType.Undestructible:
                    _meshFilter.transform.localScale = new Vector3(3f, 3f, 3f);
                    _outline.enabled = true;
                    _meshFilter.mesh = _undestructibleMesh;
                    _renderer.material = _undestructibleMaterial;
                    break;
            }
        }
        private void SetMediumDifficultyType(int extralifes, int extrabigProbability) 
        {
            if (Random.Range(0, 100) < GameManager.Instance.CrownProbability)
                _type = TMeteoriteType.Crown;
            else if (Random.Range(0, 100) < GameManager.Instance.BigProbability + extrabigProbability)
            {
                _type = TMeteoriteType.Big;
                _lifes = _lifes + extralifes;
            }
            else
                _type = TMeteoriteType.Default;
        }
        private void SetHardDifficultyType(int extrabigLifes, int extraBigProbability, int extraUndestructibeProbability) 
        {
            if (Random.Range(0, 100) < GameManager.Instance.CrownProbability)
                _type = TMeteoriteType.Crown;
            else if (Random.Range(0, 100) < GameManager.Instance.UndestructibleProbability * extraUndestructibeProbability)
                _type = TMeteoriteType.Undestructible;
            else if (Random.Range(0, 100) < GameManager.Instance.BigProbability + extraBigProbability)
            {
                _type = TMeteoriteType.Big;
                _lifes = _lifes + extrabigLifes;
            }
            else
                _type = TMeteoriteType.Default;
        }

        private void DestroyMeteorite()
        {
            _effectsController.PlayDestroyEffects(_type);

            _renderer.enabled = false;
            _collider.enabled = false;

            Destroy(this.gameObject, _effectsController.GetDestroyAudioLenght());
        }
        #endregion


    }
}

