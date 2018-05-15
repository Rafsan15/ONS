using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONS.Core.Entities
{
   public class Employees
    {
       [Key]
       public int EmployeeId { get; set; }

       [Required]
       public string EmployeeName { get; set; }
     
       [Required]
       public string FatherName { get; set; }
       
       [Required]
       public string MotherName { get; set; }
      
       public string Email { get; set; }
       
       [Required]
       public string PermanentAddress { get; set; }
      
       [Required]
       public string PresentAddress { get; set; }

       public string Nid { get; set; }
      
       [Required]
       public DateTime DOB { get; set; }

       [Required]
       public DateTime JoinDate { get; set; }

       [Required]
       public string Mobile { get; set; }

       [Required]
       public string Designation { get; set; }

       public string Propic { get; set; }

       [Required]
       public double Salary { get; set; }

       public double Due { get; set; }

       public double IsNewMonth { get; set; }
    }
}
