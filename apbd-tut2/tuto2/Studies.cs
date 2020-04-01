using System;
using System.Collections.Generic;
using System.Text;

namespace tuto2
{
    [Serializable]
    public class Studies
    {
        public string Name { get; set; }
        public string Mode { get; set; }

        public Studies(string name, string mode)
        {
            this.Name = name;
            this.Mode = mode;
        }
        public Studies() { }

        public override bool Equals(object obj)
        {
            return obj is Studies studies &&
                   Name == studies.Name &&
                   Mode == studies.Mode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Mode);
        }

        public override string ToString()
        {
            return Name + "," + Mode;
        }
    }
}
