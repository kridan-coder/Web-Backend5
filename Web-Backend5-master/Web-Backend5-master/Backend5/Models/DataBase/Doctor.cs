using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class Doctor
    {
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        public String Specialty { get; set; }

        public ICollection<HospitalDoctor> Hospitals { get; set; }

        public ICollection<DoctorPatient> Patients { get; set; }

    }
}
