using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Catkey.StarSlayer.Managers;
using System.Collections;
using Catkey.StarSlayer.Server;

namespace Catkey.StarSlayer.GPGS 
{
    public class GPGSManager : MonoBehaviour
    {
        private const float AuthenticationWaitTimeSeconds = 5;

        public void SignIntoGPGS() 
        {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();

            PlayGamesPlatform.Instance.Authenticate((status) =>
            {
                if(status == SignInStatus.Success) 
                    BackendConnection.Instance.CheckUser(Social.localUser.id);
                else 
                    StartCoroutine(WaitForAuthenticationCoroutine());
            });
        }

        public void SignIntoGameCenter() 
        {
            Social.localUser.Authenticate(ProcessAuthentication);
        }

        private void ProcessAuthentication(bool success) 
        {
            if (success)
                BackendConnection.Instance.CheckUser(Social.localUser.id);
            else
                StartCoroutine(WaitForAuthenticationCoroutine());
        }

        private IEnumerator WaitForAuthenticationCoroutine()
        {
            var startTime = Time.realtimeSinceStartup;

            while (!Social.localUser.authenticated)
            {
                // X seconds have passed and we are still not authenticated, time to give up.
                if (Time.realtimeSinceStartup - startTime > AuthenticationWaitTimeSeconds)
                    break;

                yield return null;
            }

            if (Social.localUser.authenticated)
                BackendConnection.Instance.CheckUser(Social.localUser.id);
            else
                BackendConnection.Instance.CheckUser("unityuser");
                
        }
    }
}

