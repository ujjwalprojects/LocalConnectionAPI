﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblPolicie
    {
        [Key]
        public long PolicyID { get; set; }
        public string PolicyTitle { get; set; }
    }
}
