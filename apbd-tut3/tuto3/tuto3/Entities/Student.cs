using System;
using System.Collections.Generic;

namespace tuto3.Entities
{
    public partial class Student
    {
        public string IndexNumber { get; set; }
        public string Password { get; set; }
        public int IdRole { get; set; }
        public string Salt { get; set; }
        public string RefreshToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int IdEnrollment { get; set; }

        public virtual Enrollment IdEnrollmentNavigation { get; set; }
        public virtual StudentRole IdRoleNavigation { get; set; }
    }
}
