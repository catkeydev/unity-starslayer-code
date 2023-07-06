using Catkey.StarSlayer.Managers;
using UnityEngine;

namespace Catkey.StarSlayer.Core 
{
    public class PlayerEffects : MonoBehaviour
    {
        [Header("Animation")]
        public Animation _animation;
        public AnimationClip _moveLeftAnimationClip;
        public AnimationClip _moveRightAnimationClip;
        public AnimationClip _playerShootAnimationClip;
        public AnimationClip _dieAnimationClip;

        [Header("Effects")]
        public ParticleSystem _shootParticleSystem;
        public ParticleSystem _explosionParticleSystem;
        public ParticleSystem[] _motionParticleSystems;

        /// <summary>
        /// Play move left animation
        /// </summary>
        public void PlayMoveLeftAnimation()
        {
            _animation.clip = _moveLeftAnimationClip;
            _animation.Play();

            AudioManager.Instance.PlayMoveLeftAudioClip();
        }
        /// <summary>
        /// Play move right animation
        /// </summary>
        public void PlayMoveRightAnimation()
        {
            _animation.clip = _moveRightAnimationClip;
            _animation.Play();

            AudioManager.Instance.PlayMoveRightAudioClip();
        }
        /// <summary>
        /// Play die animation
        /// </summary>
        public void PlayDieAnimation() 
        {
            Instantiate(_explosionParticleSystem, transform.position, Quaternion.identity).Play();
            foreach (ParticleSystem particleSystem in _motionParticleSystems)
                particleSystem.Stop();

            _animation.clip = _dieAnimationClip;
            _animation.Play();

            AudioManager.Instance.PlayExplosionAudioClip();
        }
        /// <summary>
        /// Play shoot animation
        /// </summary>
        /// <param name="firePointPosition">Particle starting position</param>
        public void PlayShootAnimation(Vector3 firePointPosition)
        {
            Instantiate(_shootParticleSystem, firePointPosition, Quaternion.identity).Play();
            _animation.PlayQueued("PlayerShoot_Animation");

            AudioManager.Instance.PlayShotAudio();
        }
    }
}

