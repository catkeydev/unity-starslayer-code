using UnityEngine;

namespace Catkey.StarSlayer.Menu
{
    [CreateAssetMenu(fileName = "SpceshipColor", menuName = "Spaceship color/New color", order = 1)]
    public class SpaceshipColorData : ScriptableObject
    {
        public Material material;
        public string id;
        public int price;
    }

}
