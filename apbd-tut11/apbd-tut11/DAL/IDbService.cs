using apbd_tut11.DTOs.Requests;
using apbd_tut11.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut11.DAL
{
    public interface IDbService
    {
        public ClinicDbContext DbContext { get; set; }

        public IEnumerable<DoctorModel> GetDoctors();
        public void AddDoctor(AddDoctorRequest request);
        public void DeleteDoctor(int idDoctor);
        public void UpdateDoctor(UpdateDoctorRequest request);
        
    }
}
