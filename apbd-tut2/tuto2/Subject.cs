using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace tuto2
{
    [Serializable]
    [XmlType(TypeName = "studies")]
    public class Subject
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public int NumberOfStudents { get; set; }
        
        public Subject() { }

        public Subject(string name, int number)
        {
            this.Name = name;
            this.NumberOfStudents = number;
        }

        public override bool Equals(object obj)
        {
            return obj is Subject subject &&
                   Name == subject.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
