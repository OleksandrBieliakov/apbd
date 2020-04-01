using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using tuto3.Models;

namespace tuto3.DAL
{
    public class MsSqlDbService : IDbService
    {
        private const string ConnectionString = "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s18885;Integrated Security=True";

        public int DeleteStudnet(string indexNumber)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "delete from student where indexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);

            connection.Open();
            return command.ExecuteNonQuery();
        }

        public Enrollment GetEnrollment(string indexNumber)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "select st.name as studies, semester, startDate " +
                                    "from Student s " +
                                    "join Enrollment e " +
                                    "on s.IdEnrollment = e.IdEnrollment " +
                                    "join Studies st " +
                                    "on e.idstudy = st.IdStudy " +
                                    "where s.indexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);

            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                Enrollment student = new Enrollment
                {
                    StartDate = DateTime.Parse(dataReader["startDate"].ToString()),
                    Studies = dataReader["studies"].ToString(),
                    Semester = int.Parse(dataReader["semester"].ToString())
                };
                return student;
            }

            return null;
        }

        public Student GetStudent(string indexNumber)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "select indexNumber, firstName, LastName, birthDate, st.name as studies, semester " +
                                    "from Student s " +
                                    "join Enrollment e " +
                                    "on s.IdEnrollment = e.IdEnrollment " +
                                    "join Studies st " +
                                    "on e.idstudy = st.IdStudy " +
                                    "where s.indexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);

            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                Student student = new Student
                {
                    IndexNumber = dataReader["indexNumber"].ToString(),
                    FirstName = dataReader["firstName"].ToString(),
                    LastName = dataReader["lastName"].ToString(),
                    BirthDate = DateTime.Parse(dataReader["birthDate"].ToString()),
                    Studies = dataReader["studies"].ToString(),
                    Semester = int.Parse(dataReader["semester"].ToString())
                };
                return student;
            }

            return null;
        }

        public IEnumerable<Student> GetStudents()
        {
            List<Student> students = new List<Student>();

            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "select indexNumber, firstName, LastName, birthDate, st.name as studies, semester " +
                                    "from Student s " +
                                    "join Enrollment e " +
                                    "on s.IdEnrollment = e.IdEnrollment " +
                                    "join Studies st " +
                                    "on e.idstudy = st.IdStudy "
            };

            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                Student student = new Student
                {
                    IndexNumber = dataReader["indexNumber"].ToString(),
                    FirstName = dataReader["firstName"].ToString(),
                    LastName = dataReader["lastName"].ToString(),
                    BirthDate = DateTime.Parse(dataReader["birthDate"].ToString()),
                    Studies = dataReader["studies"].ToString(),
                    Semester = int.Parse(dataReader["semester"].ToString())
                };
                students.Add(student);
            }

            return students;
        }

        public int InsertOrUpdate(Student student)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "update student set firstName=@firstName, lastName=@lastName, birthDate=@birthDate, idEnrollment=@idEnrollment " +
                                "where indexNumber=@indexNumber " +
                                @"if @@rowcount=0 " +
                                "insert into student (indexNumber, firstName, lastName, birthDate, idEnrollment) " +
                                "values(@indexNumber, @firstName, @lastName, @birthDate, @idEnrollment)"
            };
            command.Parameters.AddWithValue("indexNumber", student.IndexNumber);
            command.Parameters.AddWithValue("firstName", student.FirstName);
            command.Parameters.AddWithValue("lastName", student.LastName);
            command.Parameters.AddWithValue("birthDate", student.BirthDate.ToString());
            command.Parameters.AddWithValue("idEnrollment", student.Semester);

            connection.Open();
            return command.ExecuteNonQuery();
        }

        public int InsertStudent(Student student)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "insert into student (indexNumber, firstName, lastName, birthDate, idEnrollment) " +
                                    "values(@indexNumber, @firstName, @lastName, @birthDate, @idEnrollment)"
            };
            command.Parameters.AddWithValue("indexNumber", student.IndexNumber);
            command.Parameters.AddWithValue("firstName", student.FirstName);
            command.Parameters.AddWithValue("lastName", student.LastName);
            command.Parameters.AddWithValue("birthDate", student.BirthDate.ToString());
            command.Parameters.AddWithValue("idEnrollment", student.Semester);

            connection.Open();
            return command.ExecuteNonQuery();
        }
    }
}
