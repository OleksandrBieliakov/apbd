using apbd_tut11.DTOs.Requests;
using apbd_tut11.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut11.DAL
{
    public class EFDbService : IDbService
    {
        public ClinicDbContext DbContext { get; set; }

        public IEnumerable<DoctorModel> GetDoctors()
        {
            return DbContext.Doctor.Select(d => new DoctorModel
            {
                IdDoctor = d.IdDoctor,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email
            }).ToList();
        }

        public void AddDoctor(AddDoctorRequest request)
        {
            DbContext.Add(new Doctor
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            });
            DbContext.SaveChanges();
        }

        public void DeleteDoctor(int idDoctor)
        {
            var doctor = (new Doctor { IdDoctor = idDoctor });
            DbContext.Attach(doctor);
            DbContext.Remove(doctor);
            DbContext.SaveChanges();
        }

        public void UpdateDoctor(UpdateDoctorRequest request)
        {
            var doctor = new Doctor { IdDoctor = request.IdDoctor };
            DbContext.Attach(doctor);
            if (request.FirstName != null)
            {
                doctor.FirstName = request.FirstName;
                DbContext.Entry(doctor).Property("FirstName").IsModified = true;
            }
            if (request.LastName != null)
            {
                doctor.LastName = request.LastName;
                DbContext.Entry(doctor).Property("LastName").IsModified = true;
            }
            if (request.Email != null)
            {
                doctor.Email = request.Email;
                DbContext.Entry(doctor).Property("Email").IsModified = true;
            }
            DbContext.SaveChanges();
        }
    }
}
