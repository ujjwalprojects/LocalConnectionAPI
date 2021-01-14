﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblMstDestination
    {
        [Key]
        public long DestinationID { get; set; }
        [Required]
        public long CountryID { get; set; }
        [Required]
        public long StateID { get; set; }
        [Required]
        public string DestinationName { get; set; }
        public string DestinationDesc { get; set; }
        public string DestinationImagePath { get; set; }
    }
}
