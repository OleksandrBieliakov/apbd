using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tuto3.DTOs.Requests;
using tuto3.DTOs.Responses;
using tuto3.Models;

namespace tuto3.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        public Enrollment GetEnrollment(string indexNumber);
        public Enrollment GetEnrollment(int idEnrollment);
        public Student GetStudent(string indexNumber);
        public int DeleteStudnet(string indexNumber);
        public int InsertStudent(Student student);
        public int InsertOrUpdate(Student student);
        public StudentEnrollmentRes EnrollStudent(StudentEnrollmentReq req);
        public StudnetsPromotionRes PromoteStudnets(StudnetsPromotionReq req);
        public IEnumerable<LogEntry> GetLog();
        public int InsertLogEntry(LogEntry logEntry);
        public StudentCredentialsRes GetCredentials(string indexNumber);
        public StudentByRefreshTokenRes StudentByRefreshToken(string refreshToken);
        public int SetRefreshTocken(SetRefreshTokenReq req);
        public int UpgradeStudentPassword(UpgradeStudentPasswordReq req);

    }
}
