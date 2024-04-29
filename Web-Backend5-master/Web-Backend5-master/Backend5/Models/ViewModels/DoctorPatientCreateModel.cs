using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class DoctorPatientCreateModel
    {
        public Int32 CurrentDoctorId { get; set; }

        public Int32 PatientId { get; set; }
    }
}
