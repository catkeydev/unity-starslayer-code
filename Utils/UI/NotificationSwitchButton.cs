using UnityEngine;

namespace Catkey.StarSlayer.Utils 
{
    public class NotificationSwitchButton : SwitchButton
    {
        public override void SwitchChange()
        {
            base.SwitchChange();

            if (_isSwitchOn)
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.Notifications, 0);
            }
            else
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.Notifications, 1);
            }
        }
    }
}
