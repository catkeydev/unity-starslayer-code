using UnityEngine;

namespace Catkey.StarSlayer.Menu 
{
    public class Spaceship
    {
        public Mesh Mesh;
        public string Name;
        public bool Locked;

        public Spaceship(Mesh mesh, string name, bool locked)
        {
            Mesh = mesh;
            Name = name;
            Locked = locked;
        }
    }
}
