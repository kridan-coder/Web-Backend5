﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class DiagnosisCreateModel
    {
        [Required]
        [MaxLength(200)]
        public String Type { get; set; }

        public String Complications { get; set; }

        public String Details { get; set; }

        public Int32 PatientId { get; set; }

        public Int32 Id { get; set; }

    }
}
