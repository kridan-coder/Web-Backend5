﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class PlacementCreateModel
    {
        [Required]
        public Int32 WardId { get; set; }

        [Required]
        public Int32 Bed { get; set; }
        //public Int32 PatientId { get; set; }


    }
}
