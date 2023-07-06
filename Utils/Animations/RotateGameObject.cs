using UnityEngine;

namespace Catkey.StarSlayer.Utils 
{
    public class RotateGameObject : MonoBehaviour
    {
        [SerializeField] float _xRotationsPerMinute = 1f;
        [SerializeField] float _yRotationPerMinute = 1f;
        [SerializeField] float _zRotationPerMinute = 1f;

        [SerializeField] bool _randomize = false;

        bool _initialzied = false;

        public void InitializeRotation(float x, float y, float z) 
        {
            Debug.Log("Initalize rotation");
            _xRotationsPerMinute = x;
            _yRotationPerMinute = y;
            _zRotationPerMinute = z;
            _initialzied = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (_randomize && !_initialzied) 
            {
                _xRotationsPerMinute = Random.Range(5, _xRotationsPerMinute);
                _yRotationPerMinute = Random.Range(5, _yRotationPerMinute);
                _zRotationPerMinute = Random.Range(5, _zRotationPerMinute);
            }

            // Degrees per frame ^-1 = seconds frame^-1 / seconds minute ^-1 * degrees rotation ^-1 * rotation per minute ^-1
            float xDegreesPerFrame = Time.deltaTime / 60 * 360 * _xRotationsPerMinute;
            transform.RotateAround(transform.position, transform.right, xDegreesPerFrame);
            float yDegreesPerFrame = Time.deltaTime / 60 * 360 * _yRotationPerMinute;
            transform.RotateAround(transform.position, transform.up, yDegreesPerFrame);
            float zDegreesPerFrame = Time.deltaTime / 60 * 360 * _zRotationPerMinute;
            transform.RotateAround(transform.position, transform.forward, zDegreesPerFrame);
        }
    }
}
