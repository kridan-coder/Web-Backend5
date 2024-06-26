﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class WardCreateModel
    {
        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        public Int32 HospitalId { get; set; }
    }
}
