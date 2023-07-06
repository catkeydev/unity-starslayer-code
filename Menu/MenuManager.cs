using Catkey.StarSlayer.Server;
using Catkey.StarSlayer.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Catkey.StarSlayer.Managers 
{
    public class MenuManager : Utils.Singleton<MenuManager>
    {
        [SerializeField] private int _frameRate;

        private void Start()
        {
            Application.targetFrameRate = _frameRate;
            InitializeSavedAudioSettings();

            if (BackendConnection.Instance != null)
                BackendConnection.Instance.UpdateMenuData();
            else
                Debug.LogWarning("[MenuManager] BackendConnection manager not found.");
        }

        #region Animations
        [Header("Animations")]
        [SerializeField] private FadeAnimationController fadeAnimationController;
        [SerializeField] private BasicAnimationController _shopBasicAnimationController;
        [SerializeField] private BasicAnimationController _settingsBasicAnimationController;
        [SerializeField] private BasicAnimationController _playBasicAnimationController;
        [SerializeField] private BasicAnimationController _homeBasicAnimationController;

        public void LoadGameScene(int index) 
        {
            StartCoroutine(LoadScene(index));
        }

        public void ShowLeaderboard()
        {
            Social.ShowLeaderboardUI();
        }


        private IEnumerator LoadScene(int index) 
        {
            PlayFadeOutAnimation();
            PlayExitButtonAnimations();
            yield return new WaitForSeconds(fadeAnimationController.FadeOutDuration);
            SceneManager.LoadScene(index);
        }

        private void PlayFadeOutAnimation() 
        {
            StartCoroutine(fadeAnimationController.PlayFadeOut(false));
        }

        private void PlayExitButtonAnimations() 
        {
            _playBasicAnimationController.PlayExitAnimation();
            _shopBasicAnimationController.PlayExitAnimation();
            _homeBasicAnimationController.PlayExitAnimation();
            _settingsBasicAnimationController.PlayExitAnimation();
        }
        #endregion

        #region Sounds
        [Header("Sound")]
        [SerializeField] private AudioSource _soundAudioSource;
        [SerializeField] private AudioSource _musicAudioSource;
        private void InitializeSavedAudioSettings() 
        {
            if (PlayerPrefs.HasKey(PlayerPrefKeys.SoundKey))
                _soundAudioSource.volume = PlayerPrefs.GetFloat(PlayerPrefKeys.SoundKey);
            else
                _soundAudioSource.volume = 1.0f;

            if(PlayerPrefs.HasKey(PlayerPrefKeys.MusicKey))
                _musicAudioSource.volume = PlayerPrefs.GetFloat(PlayerPrefKeys.MusicKey);
            else
                _musicAudioSource.volume = 1.0f;
        }

        public void UpdateSoundAudioSourceVolume(float value) 
        {
            _soundAudioSource.volume = value;
        }

        public void UpdateMusicAudioSourceVolume(float value) 
        {
            _musicAudioSource.volume = value;
        }
        #endregion
    }
}

