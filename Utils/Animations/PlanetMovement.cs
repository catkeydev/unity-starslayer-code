using UnityEngine;

namespace Catkey.StarSlayer.Utils 
{
    public class PlanetMovement : MonoBehaviour
    {
        public Vector3 _direction;
        public Vector3 _velocity;
        

        // Update is called once per frame
        void Update()
        {
            transform.position += _direction + _velocity * Time.deltaTime;
        }
    }

}
