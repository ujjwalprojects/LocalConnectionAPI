﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblTourGuide
    {
        [Key]
        public long TourGuideID { get; set; }
        [Required]
        public string TourGuideName { get; set; }
        [Required]
        public string TourGuideDesc { get; set; }
        [Required]
        public string TourGuideLink { get; set; }
    }
}
