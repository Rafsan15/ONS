using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONS.Core.Entities
{
   public class CostLog
    {
        [Key]
        public int CostId { get; set; }
        [Required]
        public string CostName { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public string IssuedBy { get; set; }
        [Required]
        public DateTime CostDate { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}
