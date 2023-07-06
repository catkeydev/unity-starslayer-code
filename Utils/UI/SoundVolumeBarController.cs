using Catkey.StarSlayer.Managers;
using UnityEngine;

namespace Catkey.StarSlayer.Utils
{
    public class SoundVolumeBarController : VolumeBarControl
    {
        private void Start()
        {
            if(PlayerPrefs.HasKey(PlayerPrefKeys.SoundKey)) 
            {
                slider.value = PlayerPrefs.GetFloat(PlayerPrefKeys.SoundKey);
                slider.onValueChanged.Invoke(slider.value);
            }
            else 
            {
                slider.value = 1;
                slider.onValueChanged.Invoke(slider.value);
            }
        }

        public void SoundScrolBarValueChange()
        {
            PlayerPrefs.SetFloat(PlayerPrefKeys.SoundKey, slider.value);
            MenuManager.Instance.UpdateSoundAudioSourceVolume(slider.value);
            base.ScrollBarValueChange();
        }
    }
}

