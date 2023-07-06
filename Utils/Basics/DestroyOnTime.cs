using UnityEngine;

namespace Catkey.StarSlayer.Utils 
{
    public class DestroyOnTime : MonoBehaviour
    {
        [SerializeField] private float _time = 5.0f;

        void Start() { Destroy(this.gameObject, _time); }
    }
}

