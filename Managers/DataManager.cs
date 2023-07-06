using Catkey.StarSlayer.Menu;
using UnityEngine;

namespace Catkey.StarSlayer.Managers 
{
    public class DataManager : Utils.Singleton<DataManager>
    {
        [Header("Spaceship data")]
        public SpaceshipData[] SpaceshipData;
        public SpaceshipColorData[] SpaceshipColorData;
    }
}
