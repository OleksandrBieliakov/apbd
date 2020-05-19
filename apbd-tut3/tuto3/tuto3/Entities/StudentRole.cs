using System;
using System.Collections.Generic;

namespace tuto3.Entities
{
    public partial class StudentRole
    {
        public StudentRole()
        {
            Student = new HashSet<Student>();
        }

        public int IdRole { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<Student> Student { get; set; }
    }
}
