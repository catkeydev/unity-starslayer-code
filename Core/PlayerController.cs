using Catkey.StarSlayer.Managers;
using UnityEngine;

namespace Catkey.StarSlayer.Core
{
    public class PlayerController : MonoBehaviour
    {
        public bool SlicedThisFrame {  get; private set; }
        public bool TouchedThisFrame { get; private set; }
        public bool ShootedThisTouch { get; private set; }
        #region Variables
        [Header("Components")]
        [SerializeField] private PlayerEffects _effects;
        [SerializeField] private BulletController _bulletController;
        [SerializeField] private Transform _bulletFirePoint;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        [Header("Movement properties")]
        [SerializeField] private float _moveDistance = 1.0f;
        [SerializeField] private float _initialSpeed = 1.0f;
        [SerializeField] private float _maxMovementSpeed;

        [Header("Shoot properties")]
        [SerializeField] private float _damage = 1;
        [SerializeField] private float _maxDamage = 2;
        [SerializeField] private float _bulletForce = 15.0f;
        [SerializeField] private float _initialShootCooldown = 0.5f;
        [SerializeField] private float _minShootCooldown = 0.1f;

        [Header("Touch parameters")]
        [SerializeField] private float _sliceTouchCooldown = 0.2f;

        [Header("Screen limits")]
        [SerializeField] private float _minX = -2.2f;
        [SerializeField] private float _maxX = 2.2f;


        private Vector2 _initialFingerPosition = Vector2.zero;
        private Vector2 _sliceDirection = Vector2.zero;        
        private Vector2 _targetMovePosition = Vector2.zero;

        private float _elapsedTimeSlice = 0;
        private float _shootCooldown;
        private float _speed;

        private bool _move = false;
        #endregion

        #region Unity Loop methods
        private void Start()
        {
            SlicedThisFrame = false;
            TouchedThisFrame = false;
            _shootCooldown = _initialShootCooldown;
            _speed = _initialSpeed;
        }
        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.Gamestate != Utils.GameState.Playing)
                return;

            GatherInput();
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.Gamestate != Utils.GameState.Playing)
                return;

            Move();
        }
        #endregion

        #region Private methods
        private void GatherInput() 
        {
            TouchedThisFrame = false;
            _elapsedTimeSlice += Time.deltaTime;

            if (Input.touchCount > 0)
            {
                SlicedThisFrame = false;

                Touch touch = Input.GetTouch(0);
                // If touch has began get initial finger position
                if (touch.phase == TouchPhase.Began)
                {
                    ShootedThisTouch = false;
                    TouchedThisFrame = true;
                    _initialFingerPosition = touch.position;
                }
                // If touch have moved update slice direction
                if (touch.phase == TouchPhase.Moved)
                {
                    if (_initialFingerPosition.x < touch.position.x)
                    {
                        _sliceDirection = Vector2.right;
                    }
                    if (_initialFingerPosition.x > touch.position.x)
                    {
                        _sliceDirection = Vector2.left;
                    }
                    if(_elapsedTimeSlice > _sliceTouchCooldown && !_move && IsPlayerInsideLimits()) 
                    {
                        SlicedThisFrame = true;
                        Slice();
                    }
                }
                // If touch is in screen then shoot
                if(touch.phase == TouchPhase.Stationary) 
                {
                    if(!SlicedThisFrame && !_move && !ShootedThisTouch) 
                    {
                        ShootedThisTouch = true;
                        Shoot();
                    }
                }
                if(touch.phase == TouchPhase.Ended) 
                {
                    ShootedThisTouch = false;
                }
            }
        }

        private void Shoot() 
        {
            _effects.PlayShootAnimation(_bulletFirePoint.position);
            Instantiate(_bulletController).InitializeBullet(_bulletFirePoint.position, Vector3.forward, _bulletForce, _damage);
        }
        /// <summary>
        /// Get target move position, set move to true and start moe animation.
        /// </summary>
        private void Slice()
        {
            _elapsedTimeSlice = 0;
            _targetMovePosition = transform.position + _moveDistance * (Vector3)_sliceDirection;
            _move = true;

            if(_sliceDirection == Vector2.left) 
            {
                _effects.PlayMoveLeftAnimation();
            }
            if(_sliceDirection == Vector2.right) 
            {
                _effects.PlayMoveRightAnimation();
            }
        }
        /// <summary>
        /// Move spaceship to desired position. Only called if move is true (slice method)
        /// </summary>
        private void Move()
        {
            if (!_move)
                return;

            // Check if target position has been reached
            if(Vector3.Distance((Vector3)_targetMovePosition, transform.position) < 0.01f) 
            {
                _move = false;
                _targetMovePosition = Vector3.zero;
                _sliceDirection = Vector2.zero;
                return;
            }

            // Move our position a step closer to the target.
            float step = _speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, _targetMovePosition, step);
        }
        /// <summary>
        /// Check if player is inside screen limits
        /// </summary>
        /// <returns></returns>
        private bool IsPlayerInsideLimits()
        {
            Vector3 _predictedPosition = transform.position + _moveDistance * (Vector3)_sliceDirection;

            if (_predictedPosition.x <= _minX) return false;
            else if (_predictedPosition.x >= _maxX) return false;
            return true;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Increase movement speed
        /// </summary>
        /// <param name="value"></param>
        public void AddMovementSpeed(float value) 
        {
            _speed += value;
            _speed = Mathf.Clamp(_speed, _initialSpeed, _maxMovementSpeed);
        }

        /// <summary>
        /// Increase attack speed
        /// </summary>
        /// <param name="value"></param>
        public void AddAttackSpeed(float value) 
        {
            _shootCooldown -= value;
            _shootCooldown = Mathf.Clamp(_shootCooldown, _minShootCooldown, _initialShootCooldown);
        }

        /// <summary>
        /// Incerase damage.
        /// </summary>
        /// <param name="damage"></param>
        public void AddDamage(float damage) 
        {
            _damage += damage;
            _damage = Mathf.Clamp(_damage, 0, _maxDamage);
        }

        public void PlayDieAniamtion() { _effects.PlayDieAnimation(); }
        public MeshFilter GetMeshFilter() { return _meshFilter; }
        public MeshRenderer GetMeshRenderer() { return _meshRenderer; }
        #endregion
    }
}