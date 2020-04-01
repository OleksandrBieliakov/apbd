using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace tuto2
{
    [Serializable]
    public class Student
    {
        [XmlAttribute(AttributeName = "IndexNumber")]
        [JsonPropertyName("IndexNumber")]
        public string Index { get; set; }
        [XmlElement(ElementName = "Fname")]
        [JsonPropertyName("Fname")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Lname")]
        [JsonPropertyName("Lname")]
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string MothersName { get; set; }
        public string FathersName { get; set; }
        public Studies Studies { get; set; }


        public Student(string name, string surname, Studies studies, string index, DateTime birthDate, string email, string mothersName, string fathersName)
        {
            this.Name = name;
            this.Surname = surname;
            this.Studies = studies;
            this.Index = index;
            this.BirthDate = birthDate;
            this.Email = email;
            this.MothersName = mothersName;
            this.FathersName = fathersName;
        }
        public Student() { }


        public override bool Equals(object obj)
        {
            return obj is Student student &&
                   Index == student.Index &&
                   Name == student.Name &&
                   Surname == student.Surname &&
                   BirthDate == student.BirthDate &&
                   Email == student.Email &&
                   MothersName == student.MothersName &&
                   FathersName == student.FathersName &&
                   EqualityComparer<Studies>.Default.Equals(Studies, student.Studies);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Index, Name, Surname, BirthDate, Email, MothersName, FathersName, Studies);
        }

        public override string ToString()
        {
            return Name + "," + Surname + "," + Studies.ToString() + "," + Index + "," + BirthDate.ToString() + "," + Email + "," + MothersName + "," + FathersName;
        }
    }
}
