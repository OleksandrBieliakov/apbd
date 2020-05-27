using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut11.Models
{
    public partial class ClinicDbContext : DbContext
    {
        public ClinicDbContext()
        {
        }

        public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<Medicament> Medicament { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Prescription> Prescription { get; set; }
        public virtual DbSet<PrescriptionMedicament> PrescriptionMedicament { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.IdDoctor)
                    .HasName("Doctor_pk");

                entity.Property(e => e.IdDoctor)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                    .IsRequired() //not null
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(e => e.IdMedicament)
                    .HasName("Medicament_pk");

                entity.Property(e => e.IdMedicament)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.IdPatient)
                    .HasName("Patient_pk");

                entity.Property(e => e.IdPatient)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Birthdate).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.IdPrescription)
                    .HasName("Prescription_pk");

                entity.Property(e => e.IdPrescription)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.HasOne(d => d.IdDoctorNavigation)
                    .WithMany(p => p.Prescription)
                    .HasForeignKey(d => d.IdDoctor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Prescription_Doctor");

                entity.HasOne(d => d.IdPatientNavigation)
                    .WithMany(p => p.Prescription)
                    .HasForeignKey(d => d.IdPatient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Prescription_Patient");
            });

            modelBuilder.Entity<PrescriptionMedicament>(entity =>
            {
                entity.HasKey(e => new { e.IdMedicament, e.IdPrescription })
                    .HasName("Prescription_Medicament_pk");

                entity.ToTable("Prescription_Medicament");

                entity.Property(e => e.Details)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.IdMedicamentNavigation)
                    .WithMany(p => p.PrescriptionMedicament)
                    .HasForeignKey(d => d.IdMedicament)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Prescription_Medicament_Medicament");

                entity.HasOne(d => d.IdPrescriptionNavigation)
                    .WithMany(p => p.PrescriptionMedicament)
                    .HasForeignKey(d => d.IdPrescription)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Prescription_Medicament_Prescription");
            });


            var doctorsSeed = new List<Doctor>();
            doctorsSeed.Add(new Doctor { IdDoctor = 1, FirstName = "DocName1", LastName = "DocLName1", Email = "doc1@clinic.org" });
            doctorsSeed.Add(new Doctor { IdDoctor = 2, FirstName = "DocName2", LastName = "DocLName2", Email = "doc2@clinic.org" });
            doctorsSeed.Add(new Doctor { IdDoctor = 3, FirstName = "DocName3", LastName = "DocLName3", Email = "doc3@clinic.org" });
            modelBuilder.Entity<Doctor>().HasData(doctorsSeed);

            var medicamentsSeed = new List<Medicament>();
            medicamentsSeed.Add(new Medicament { IdMedicament = 1, Name = "Med1", Description = "Des1", Type = "Type1" });
            medicamentsSeed.Add(new Medicament { IdMedicament = 2, Name = "Med2", Description = "Des2", Type = "Type2" });
            medicamentsSeed.Add(new Medicament { IdMedicament = 3, Name = "Med3", Description = "Des3", Type = "Type3" });
            modelBuilder.Entity<Medicament>().HasData(medicamentsSeed);

            var patientSeed = new List<Patient>();
            patientSeed.Add(new Patient { IdPatient = 1, FirstName = "PatName1", LastName = "PatLName1", Birthdate = DateTime.Now.AddYears(-21) });
            patientSeed.Add(new Patient { IdPatient = 2, FirstName = "PatName2", LastName = "PatLName2", Birthdate = DateTime.Now.AddYears(-22) });
            patientSeed.Add(new Patient { IdPatient = 3, FirstName = "PatName3", LastName = "PatLName3", Birthdate = DateTime.Now.AddYears(-23) });
            modelBuilder.Entity<Patient>().HasData(patientSeed);

            var prescriptionsSeed = new List<Prescription>();
            prescriptionsSeed.Add(new Prescription { IdPrescription = 1, Date = DateTime.Now.AddDays(-1), DueDate = DateTime.Now.AddMonths(1), IdPatient = 1, IdDoctor = 1});
            prescriptionsSeed.Add(new Prescription { IdPrescription = 2, Date = DateTime.Now.AddDays(-2), DueDate = DateTime.Now.AddMonths(2), IdPatient = 2, IdDoctor = 2 });
            prescriptionsSeed.Add(new Prescription { IdPrescription = 3, Date = DateTime.Now.AddDays(-3), DueDate = DateTime.Now.AddMonths(3), IdPatient = 3, IdDoctor = 3 });
            modelBuilder.Entity<Prescription>().HasData(prescriptionsSeed);

            var prescriptions_medicamentsSeed = new List<PrescriptionMedicament>();
            prescriptions_medicamentsSeed.Add(new PrescriptionMedicament { IdPrescription = 1, IdMedicament = 1, Dose = 1, Details = "Det1"});
            prescriptions_medicamentsSeed.Add(new PrescriptionMedicament { IdPrescription = 2, IdMedicament = 2, Dose = 2, Details = "Det2" });
            prescriptions_medicamentsSeed.Add(new PrescriptionMedicament { IdPrescription = 3, IdMedicament = 3, Dose = 3, Details = "Det3" });
            modelBuilder.Entity<PrescriptionMedicament>().HasData(prescriptions_medicamentsSeed);

        }

    }
}
