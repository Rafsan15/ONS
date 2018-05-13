using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONS.Core.Entities
{
    [NotMapped]
    public class Searching
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public string Key { get; set; }
        public string Address { get; set; }
        public string Bandwidth { get; set; }

    }
}
