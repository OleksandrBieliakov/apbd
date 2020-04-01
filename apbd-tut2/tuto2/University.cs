using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace tuto2
{
    [Serializable]
    public class University
    {
        [XmlAttribute]
        public DateTime CreatedAt { get; set; }
        [XmlAttribute]
        public string Author { get; set; }
        public HashSet<Student> Studetns { get; set; }

        public HashSet<Subject> ActiveStudies { get; set; }

        public University() { }
        public University(DateTime createdAt, string author, HashSet<Student> students, HashSet<Subject> studies)
        {
            this.CreatedAt = createdAt;
            this.Author = author;
            this.Studetns = students;
            this.ActiveStudies = studies;
        }

        public bool IsEmpty()
        {
            return Studetns.Count == 0;
        }
    }
}
