
using Catkey.StarSlayer.Utils;
using UnityEngine;

namespace Catkey.StarSlayer.Core 
{
    public class BulletController : MonoBehaviour
    {
        public Vector3 RawMovement { get; private set; }

        private bool _isActive = false;
        private Vector3 _direction = Vector3.zero;
        private float _force = 0.0f;
        private float _damage = 0.0f;

        #region Unity events
        private void FixedUpdate()
        {
            if (!_isActive)
                return;

            MoveBullet();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (!_isActive)
                return;

            if (collision.tag == "Enemy")
            {
                collision.GetComponent<IDamageable>().GetDamage(_damage);
                Destroy(this.gameObject);
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Initialize bullet method must be called when instantiating a new bullet
        /// </summary>
        /// <param name="spawnPosition">Starting position</param>
        /// <param name="direction">Movement direction</param>
        /// <param name="force">Movement force</param>
        /// <param name="damage">Damage done when colliding an Enemy</param>
        public void InitializeBullet(Vector3 spawnPosition, Vector3 direction, float force, float damage)
        {
            _isActive = true;
            _direction = direction;
            _force = force;
            _damage = damage;
            transform.position = spawnPosition;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Move transform position at given initialized force and direction. Multiplied by fixedDT.
        /// </summary>
        private void MoveBullet()
        {
            transform.position += _force * _direction * Time.fixedDeltaTime;
        }
        #endregion
    }

}
