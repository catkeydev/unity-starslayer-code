using System;

namespace Catkey.StarSlayer.Server 
{
    [Serializable]
    public class ColorsModel
    {
        public string name;
        public bool unlocked;

        public ColorsModel(string name, bool unlocked)
        {
            this.name = name;
            this.unlocked = unlocked;
        }
    }
}

