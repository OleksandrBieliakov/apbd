using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using tuto3.DTOs.Requests;
using tuto3.DTOs.Responses;
using tuto3.Models;

namespace tuto3.DAL
{
    public class SqlServerDbService : IDbService
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

        public StudentEnrollmentRes EnrollStudent(StudentEnrollmentReq request)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "SELECT * FROM Studies WHERE Name=@Name"
            };
            command.Parameters.AddWithValue("Name", request.Studies);

            connection.Open();

            var transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            int idEnrollment;

            try
            {
                var dr = command.ExecuteReader();
                // Check if the Study exists
                if (!dr.Read())
                {
                    dr.Close();
                    transaction.Rollback();
                    return new StudentEnrollmentRes
                    {
                        Error = "Study does not exist"
                    };
                }
                int idStudy = (int)dr["iDstudy"];

                command.CommandText = "Select * from Student where IndexNumber=@IndexNumber";
                command.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                dr.Close();
                dr = command.ExecuteReader();
                // Check student with given index exists
                if (dr.Read())
                {
                    dr.Close();
                    transaction.Rollback();
                    return new StudentEnrollmentRes
                    {
                        Error = "Student with given index exists"
                    };
                }

                command.CommandText = "SELECT * FROM Enrollment WHERE Semester=1 AND IdStudy=@IdStudy";
                command.Parameters.AddWithValue("IdStudy", idStudy);
                dr.Close();
                dr = command.ExecuteReader();

                if (dr.Read())
                {
                    idEnrollment = (int)dr["IdEnrollment"];
                    dr.Close();
                }
                // If there is no Enrollment with a given Study add new Enrollment
                else
                {
                    command.CommandText = "SELECT MAX(idEnrollment) as IdEnrollment FROM Enrollment";
                    dr.Close();
                    dr = command.ExecuteReader();
                    var nextIdEnrollment = 1;
                    if (dr.Read())
                    {
                        nextIdEnrollment = (int)dr["IdEnrollment"] + 1;
                    }
                    dr.Close();
                    var startDate = DateTime.Now;
                    command.CommandText = "INSERT INTO Enrollment(idEnrollment, idStudy, semester, startDate) " +
                                                          "values(@NextIdEnrollment, @IdStudy, 1, @StartDate)";
                    command.Parameters.AddWithValue("NextIdEnrollment", nextIdEnrollment);
                    command.Parameters.AddWithValue("StartDate", startDate);
                    command.ExecuteNonQuery();
                    idEnrollment = nextIdEnrollment;
                }

                command.CommandText = "INSERT INTO Student(IndexNumber, firstname, lastname, birthdate, idenrollment) " +
                                                            "values(@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment)";
                command.Parameters.AddWithValue("FirstName", request.FirstName);
                command.Parameters.AddWithValue("LastName", request.LastName);
                command.Parameters.AddWithValue("BirthDate", request.BirthDate);
                command.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (SqlException)
            {
                transaction.Rollback();
                return new StudentEnrollmentRes
                {
                    Error = "SQLException"
                };
            }
            return new StudentEnrollmentRes
            {
                IdEnrollment = idEnrollment,
                Semester = 1
            };
        }

        public Enrollment GetEnrollment(int idEnrollment)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "select idenrollment, name, semester, startdate " +
                                       "from enrollment e join studies s " +
                                       "on e.idstudy = s.idstudy " +
                                       "where e.idenrollment = @IdEnrollment"
            };
            command.Parameters.AddWithValue("IdEnrollment", idEnrollment);

            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                Enrollment enrollment = new Enrollment
                {
                    IdEnrollment = int.Parse(dataReader["idenrollment"].ToString()),
                    Studies = dataReader["name"].ToString(),
                    Semester = int.Parse(dataReader["semester"].ToString()),
                    StartDate = DateTime.Parse(dataReader["startdate"].ToString())
                };
                return enrollment;
            }

            return null;
        }

        public StudnetsPromotionRes PromoteStudnets(StudnetsPromotionReq req)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand("PromoteStudents")
            {
                Connection = connection,
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add(new SqlParameter("@StudiesName", req.Studies));
            command.Parameters.Add(new SqlParameter("@Semester", req.Semester));

            connection.Open();
            try
            {
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    StudnetsPromotionRes enrollment = new StudnetsPromotionRes
                    {
                        IdEnrollment = int.Parse(dataReader["idenrollment"].ToString()),
                        Studies = dataReader["name"].ToString(),
                        Semester = int.Parse(dataReader["semester"].ToString()),
                        StartDate = DateTime.Parse(dataReader["startdate"].ToString())
                    };
                    return enrollment;
                }
            }
            catch (SqlException e)
            {
                return new StudnetsPromotionRes { Error = e.Message };
            }
            return null;
        }

        public IEnumerable<LogEntry> GetLog()
        {
            List<LogEntry> log = new List<LogEntry>();

            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "select IdLogEntry, EntryTime, MethodName, PathString, QueryString, BodyString " +
                                    "from LogEntry l " +
                                    "join HttpMethod m " +
                                    "on l.IdMethod = m.IdMethod"

            };

            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                LogEntry logEntry = new LogEntry
                {
                    Id = int.Parse(dataReader["IdLogEntry"].ToString()),
                    Time = DateTime.Parse(dataReader["EntryTime"].ToString()),
                    Method = dataReader["MethodName"].ToString(),
                    Path = dataReader["PathString"].ToString(),
                    QueryString = dataReader["QueryString"].ToString(),
                    Body = dataReader["BodyString"].ToString(),
                };
                log.Add(logEntry);
            }

            return log;
        }

        public int InsertLogEntry(LogEntry logEntry)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "select isnull(max(IdLogEntry), 1) as MaxID from LogEntry"
            };
            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            int nextIdLogEntry = 1;
            if (dataReader.Read())
            {
                nextIdLogEntry = int.Parse(dataReader["MaxID"].ToString()) + 1;
            }
            dataReader.Close();

            command.CommandText = "select IdMethod from HttpMethod where MethodName=@MethodName";
            command.Parameters.AddWithValue("MethodName", logEntry.Method);

            dataReader = command.ExecuteReader();
            int idMethod = 1;
            if (dataReader.Read())
            {
                idMethod = int.Parse(dataReader["IdMethod"].ToString());
                dataReader.Close();
            }
            else
            {
                dataReader.Close();
                command.CommandText = "select isnull(max(IdMethod), 1) as MaxId from HttpMethod";
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    idMethod = int.Parse(dataReader["MaxID"].ToString()) + 1;
                }
                dataReader.Close();

                command.CommandText = "insert into HttpMethod(IdMethod, MethodName) " +
                                        "values(@IdMethodNext, @MethodName)";
                command.Parameters.AddWithValue("IdMethodNext", idMethod);
                command.ExecuteNonQuery();
            }


            command.CommandText = "insert into LogEntry(IdLogEntry, EntryTime, IdMethod, PathString, QueryString, BodyString) " +
                                    "values(@NextIdLogEntry, @EntryTime, @IdMethod, @PathString, @QueryString, @BodyString)";

            command.Parameters.AddWithValue("NextIdLogEntry", nextIdLogEntry);
            command.Parameters.AddWithValue("EntryTime", logEntry.Time);
            command.Parameters.AddWithValue("IdMethod", idMethod);
            command.Parameters.AddWithValue("PathString", logEntry.Path);
            command.Parameters.AddWithValue("QueryString", logEntry.QueryString);
            command.Parameters.AddWithValue("BodyString", logEntry.Body);

            command.ExecuteNonQuery();

            return nextIdLogEntry;
        }

        public StudentCredentialsRes GetCredentials(string indexNumber)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "select Password, Salt, RoleName " +
                                    "from Student s " +
                                    "join StudentRole sr " +
                                    "on s.IdRole = sr.IdRole " +
                                    "where s.indexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);

            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                StudentCredentialsRes res = new StudentCredentialsRes
                {
                    Password = dataReader["password"].ToString(),
                    Salt = dataReader["salt"].ToString(),
                    Role = dataReader["roleName"].ToString()
                };
                return res;
            }

            return null;
        }

        public StudentByRefreshTokenRes StudentByRefreshToken(string refreshToken)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "select s.indexNumber, sr.RoleName " +
                                    "from Student s " +
                                    "join StudentRole sr " +
                                    "on s.IdRole = sr.IdRole " +
                                    "where s.refreshToken = @refreshToken"
            };
            command.Parameters.AddWithValue("refreshToken", refreshToken);

            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                return new StudentByRefreshTokenRes
                {
                    IndexNumber = dataReader["indexNumber"].ToString(),
                    Role = dataReader["RoleName"].ToString()
                };
            }
            return null;
        }

        public int SetRefreshTocken(SetRefreshTokenReq req)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "update student " +
                                    "set refreshToken = @RefreshToken " +
                                    "where indexNumber = @IndexNumber"
            };
            command.Parameters.AddWithValue("refreshToken", req.RefreshToken);
            command.Parameters.AddWithValue("indexNumber", req.IndexNumber);

            connection.Open();
            return command.ExecuteNonQuery();

        }

        public int UpgradeStudentPassword(UpgradeStudentPasswordReq req)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "update student " +
                                    "set password = @Password, salt = @Salt " +
                                    "where indexNumber = @IndexNumber"
            };

            command.Parameters.AddWithValue("indexNumber", req.IndexNumber);
            command.Parameters.AddWithValue("password", req.Password);
            command.Parameters.AddWithValue("salt", req.Salt);

            connection.Open();

            return command.ExecuteNonQuery();
        }

    }
}
