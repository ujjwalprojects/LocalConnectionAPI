using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblMstCountries
    {
        [Key]
        public long CountryID { get; set; }
        public string CountryName { get; set; }
    }
}
