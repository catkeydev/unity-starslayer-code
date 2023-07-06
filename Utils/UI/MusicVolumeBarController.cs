using Catkey.StarSlayer.Managers;
using UnityEngine;

namespace Catkey.StarSlayer.Utils 
{
    public class MusicVolumeBarController : VolumeBarControl
    {
        private void Start()
        {
            if(PlayerPrefs.HasKey(PlayerPrefKeys.MusicKey)) 
            {
                slider.value = PlayerPrefs.GetFloat(PlayerPrefKeys.MusicKey);
                slider.onValueChanged.Invoke(slider.value);
            }
            else 
            {
                PlayerPrefs.SetFloat(PlayerPrefKeys.MusicKey, 1);
                slider.value = 1;
                slider.onValueChanged.Invoke(slider.value);
            }
        }

        public void MusicScrollBarValueChange() 
        {
            PlayerPrefs.SetFloat(PlayerPrefKeys.MusicKey, slider.value);
            MenuManager.Instance.UpdateMusicAudioSourceVolume(slider.value);
            base.ScrollBarValueChange();
        }
    }
}

