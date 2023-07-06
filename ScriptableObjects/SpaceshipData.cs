using UnityEngine;

namespace Catkey.StarSlayer.Menu 
{
    [CreateAssetMenu(fileName = "Spceship", menuName = "Spaceship/New spaceship", order =1)]
    public class SpaceshipData : ScriptableObject
    {
        public Mesh mesh;
        public string id;
        public int price;
    }
}
