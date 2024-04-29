using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend5.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend5.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Hospital> Hospitals { get; set; }

        public DbSet<HospitalPhone> HospitalPhones { get; set; }

        public DbSet<Ward> Wards { get; set; }

        public DbSet<Lab> Labs { get; set; }

        public DbSet<LabPhone> LabPhones { get; set; }

        public DbSet<HospitalLab> HospitalLabs { get; set; }

        public DbSet<HospitalDoctor> HospitalDoctors { get; set; }

        public DbSet<DoctorPatient> DoctorPatients { get; set; }

        public DbSet<WardStaff> WardStaffs { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Analysis> Analyses { get; set; }

        public DbSet<Placement> Placements { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Diagnosis> Diagnoses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Diagnosis>()
                .HasKey(x => new { x.Id, x.PatientId });

            modelBuilder.Entity<Analysis>()
                .HasKey(x => new { x.Id, x.PatientId });

            modelBuilder.Entity<WardStaff>()
                .HasKey(x => new { x.Id, x.WardId });

/*            modelBuilder.Entity<Ward>()
                .HasKey(x => new { x.Id, x.HospitalId });*/

            modelBuilder.Entity<HospitalPhone>()
                .HasKey(x => new { x.HospitalId, x.PhoneId });

            modelBuilder.Entity<LabPhone>()
                .HasKey(x => new { x.LabId, x.PhoneId });

            modelBuilder.Entity<HospitalLab>()
                .HasKey(x => new { x.HospitalId, x.LabId });

            modelBuilder.Entity<HospitalDoctor>()
                .HasKey(x => new { x.HospitalId, x.DoctorId });

            modelBuilder.Entity<DoctorPatient>()
                .HasKey(x => new { x.DoctorId, x.PatientId });

        }
    }
}
