using Catkey.StarSlayer.GPGS;
using Catkey.StarSlayer.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Catkey.StarSlayer.Managers 
{
    public class SplashSceneManager : Catkey.StarSlayer.Utils.Singleton<SplashSceneManager>
    {
        public GPGSManager GPGSManager;
        public FadeAnimationController FadeAnimationController;

        private void Start()
        {
#if PLATFORM_ANDROID
            GPGSManager.SignIntoGPGS();
#else
            GPGSManager.SignIntoGameCenter();
#endif
        }
        public void LoadMainMenuScene(int index) 
        {
            StartCoroutine(LoadMainMenu(index));
        }

        private IEnumerator LoadMainMenu(int index) 
        {
            StartCoroutine(FadeAnimationController.PlayFadeOut(false));
            yield return new WaitForSeconds(2.5f);
            SceneManager.LoadScene(index);
        }
    }
}
