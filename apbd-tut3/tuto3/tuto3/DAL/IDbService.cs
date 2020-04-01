using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tuto3.Models;

namespace tuto3.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        public Enrollment GetEnrollment(string indexNumber);
        public Student GetStudent(string indexNumber);
        public int DeleteStudnet(string indexNumber);
        public int InsertStudent(Student student);
        public int InsertOrUpdate(Student student);
    }
}
