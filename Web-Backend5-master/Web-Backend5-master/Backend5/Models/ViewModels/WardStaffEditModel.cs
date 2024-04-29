using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class WardStaffEditModel
    {
        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        public String Position { get; set; }

        public Int32 WardId { get; set; }

        public Int32 Id { get; set; }

    }
}
