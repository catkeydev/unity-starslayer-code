using UnityEngine;

namespace Catkey.StarSlayer.Utils 
{
    public class DontDestroy : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }

}
