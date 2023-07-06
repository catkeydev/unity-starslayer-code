using UnityEngine;

namespace Catkey.StarSlayer.Managers
{
    public class AudioManager : Catkey.StarSlayer.Utils.Singleton<AudioManager>
    {
        [Header("Music audio components")]
        [SerializeField] AudioSource _musicAudioSource;

        [Header("Music clips")]
        [SerializeField] AudioClip _environmentClip;

        [Header("Sound audio components")]
        [SerializeField] AudioSource _soundEffectAudioSource;
        [SerializeField] AudioSource _spaceshipAudioSource;
        [SerializeField] AudioSource _firePointAudioSource;

        [Header("Sound clips")]
        [SerializeField] AudioClip _onButtonClickClip;
        [SerializeField] AudioClip _collectAudioClip;
        [SerializeField] AudioClip _explosionAudioClip;
        [SerializeField] AudioClip[] _shootAudioClip;
        [SerializeField] AudioClip[] _moveRightAudioClips;
        [SerializeField] AudioClip[] _moveLeftAudioClips;
        [SerializeField] AudioClip[] _bulletCollisionAudioClips;
        [SerializeField] AudioClip[] _meteoriteDestroyAudioClip;

        private float _lastDestructionAudioClipLenght = 0.0f;

        #region Unity events

        public void Start()
        {
            InitializeAudioSources();
        }

        #endregion

        #region Public methods

        public void PlayButtonClick() 
        {
            _soundEffectAudioSource.clip = _onButtonClickClip;
            _soundEffectAudioSource.Play();
        }
        public void PlayExplosionAudioClip() 
        {
            _spaceshipAudioSource.clip = _explosionAudioClip;
            _spaceshipAudioSource.Play();
        }
        /// <summary>
        /// Play move audio
        /// </summary>
        public void PlayMoveLeftAudioClip() 
        {
            _spaceshipAudioSource.clip = _moveLeftAudioClips[Random.Range(0, _moveLeftAudioClips.Length)];
            _spaceshipAudioSource.Play();
        }

        /// <summary>
        /// Play move audio
        /// </summary>
        public void PlayMoveRightAudioClip()
        {
            _spaceshipAudioSource.clip = _moveRightAudioClips[Random.Range(0, _moveRightAudioClips.Length)];
            _spaceshipAudioSource.Play();
        }

        /// <summary>
        /// Play shoot audio.
        /// </summary>
        public void PlayShotAudio() 
        {
            _firePointAudioSource.clip = _shootAudioClip[Random.Range(0, _shootAudioClip.Length)];
            _firePointAudioSource.Play();
        }
        public void PlayCollectAudioClip(AudioSource audioSource) 
        {
            audioSource.clip = _collectAudioClip;
            audioSource.Play();
        }
        /// <summary>
        /// Play impact audiclip sound.
        /// </summary>
        /// <param name="impactAudioSource">Audio source attached to impacted object.</param>
        public void PlayMeteoriteImpactAudioClip(AudioSource impactAudioSource) 
        {
            impactAudioSource.clip = _bulletCollisionAudioClips[Random.Range(0, _bulletCollisionAudioClips.Length)];
            impactAudioSource.Play();
        }

        /// <summary>
        /// Play meteorite destruction audioclip
        /// </summary>
        /// <param name="impactAudioSource">Audio source attached to impacted object.</param>
        public void PlayMeteoriteDestructionAudioClip(AudioSource impactAudioSource) 
        {
            AudioClip destructionAudioClip = _meteoriteDestroyAudioClip[Random.Range(0, _meteoriteDestroyAudioClip.Length)];
            _lastDestructionAudioClipLenght = destructionAudioClip.length;

            impactAudioSource.clip = destructionAudioClip;
            impactAudioSource.Play();
        }

        public float GetLastDestructionAudioClipLenghth() { return _lastDestructionAudioClipLenght; }

        #endregion

        #region Private methods
        /// <summary>
        /// Get playerprefs sound settings (SoundVolume, MusicVolume).
        /// </summary>
        private void InitializeAudioSources()
        {
            if (PlayerPrefs.HasKey("SoundVolume"))
            {
                _soundEffectAudioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
            }
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                _musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            }

            _musicAudioSource.clip = _environmentClip;
            _musicAudioSource.Play();
        }

        #endregion
    }
}

