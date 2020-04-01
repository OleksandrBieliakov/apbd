using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tuto3.Models;

namespace tuto3.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student{FirstName="Sanya", LastName="Black", IndexNumber="s1111"},
                new Student{FirstName="Petia", LastName="Brown", IndexNumber="s2222"},
                new Student{FirstName="Vasia", LastName="Green", IndexNumber="s3333"}
            };
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }

        public Enrollment GetEnrollment(string indexNumber)
        {
            throw new NotImplementedException();
        }

        public Student GetStudent(string indexNumber)
        {
            throw new NotImplementedException();
        }

        public int DeleteStudnet(string indexNumber)
        {
            throw new NotImplementedException();
        }

        public int InsertStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public int InsertOrUpdate(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
