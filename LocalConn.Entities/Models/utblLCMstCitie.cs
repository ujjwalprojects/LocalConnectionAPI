using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCMstCitie
    {
        [Key]
        public long CityID { get; set; }
        public string CityName  { get; set; }
        public string CityIconPath { get; set; }
        public long StateID { get; set; }
        public long CountryID { get; set; }
        public bool IsPopular { get; set; }
    }
}
