using UnityEngine;

namespace Catkey.StarSlayer.Managers 
{
    public class PerformanceManager : MonoBehaviour
    {
        [SerializeField] private int _frameRate;
        private void Start()
        {
            Application.targetFrameRate = _frameRate;
        }

        public void CapFrameRate() 
        {
            Application.targetFrameRate = _frameRate;
        }
    }
}
