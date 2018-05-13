using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONS.Core.Entities
{
   public class Clients
    {
       [Key]
       public int ClientId { get; set; }

       [Required]
       public string ClientName { get; set; }
     
       [EmailAddress]
       public string ClientEmail { get; set; }
       
       [Required]
       public string Address { get; set; }
      
       [Required]
       public string PhoneNumber { get; set; }
      
       [Required]
       public string Package { get; set; }
      
       [Required]
       public double MonthlyBill { get; set; }

       [Required]
       public string UserName { get; set; }

       [Required]
       public string Password { get; set; }

       [Required]
       public string UserType { get; set; }
     
       [Required]
       public double ConnectionFee { get; set; }

       public double Others { get; set; }

       public double RouterFee { get; set; }

       public double Pay { get; set; }

       public double Due { get; set; }

       [NotMapped]
       public double Paid { get; set; }

       [NotMapped]
       public double TotalBalance { get; set; }

       [Required]
       public DateTime JoinDate { get; set; }

       public int IsValid { get; set; }


    }
}
