using System;

namespace Catkey.StarSlayer.Server
{
    [Serializable]
    public class SpaceshipsModel
    {
        public string name;
        public bool unlocked;

        public SpaceshipsModel(string name, bool unlocked)
        {
            this.name = name;
            this.unlocked = unlocked;
        }
    }
}

