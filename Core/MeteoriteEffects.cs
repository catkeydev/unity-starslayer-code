using Catkey.StarSlayer.Managers;
using Catkey.StarSlayer.Utils;
using UnityEngine;

namespace Catkey.StarSlayer.Core 
{
    public class MeteoriteEffects : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _destructionAudioSource;

        [Header("Animations")]
        [SerializeField] private Animation _animation;
        [SerializeField] private AnimationClip _meteoriteStartingAnimationCip;

        [Header("Particles")]
        [SerializeField] private ParticleSystem[] _destroyParticleSystems;
        [SerializeField] private ParticleSystem _collectParticleSystem;
        [SerializeField] private ParticleSystem _collisionParticleSystem;

        #region Public methods
        public void PlayStartingMeteoriteAnimation() 
        {
            _animation.clip = _meteoriteStartingAnimationCip;
            _animation.Play();
        }
        /// <summary>
        /// Play audio depending on type
        /// </summary>
        public void PlayDestroyEffects(TMeteoriteType type) 
        {
            if (type == TMeteoriteType.Crown) 
            {
                AudioManager.Instance.PlayCollectAudioClip(_audioSource);
                Instantiate(_collectParticleSystem, transform.position, Quaternion.identity).Play();
            }
            else 
            {
                AudioManager.Instance.PlayMeteoriteDestructionAudioClip(_destructionAudioSource);
                Instantiate(_destroyParticleSystems[Random.Range(0, _destroyParticleSystems.Length)], transform.position, Quaternion.identity).Play();
            }

        }

        /// <summary>
        /// Instantiate collision particlesystem and play audio impact
        /// </summary>
        public void PlayCollisionEffects() 
        {
            Instantiate(_collisionParticleSystem, transform.position, Quaternion.identity).Play();
            AudioManager.Instance.PlayMeteoriteImpactAudioClip(_audioSource);
        }

        /// <summary>
        /// Returns audio lenght of current clip. Used to get delay when destroying meteorite
        /// </summary>
        /// <returns></returns>
        public float GetDestroyAudioLenght() 
        {
            return AudioManager.Instance.GetLastDestructionAudioClipLenghth();
        }

        #endregion
    }
}

