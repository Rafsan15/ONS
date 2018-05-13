using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONS.Core.Entities
{
   public class CollectionInfo
    {
       [Key]
       public int CollectionId { get; set; }

       public int UserId { get; set; }

       [ForeignKey("UserId")]
       public Users Users;

       [Required]
       public double Amount { get; set; }
   
       [Required]
       public DateTime CollectionDate { get; set; }

       public string UserName { get; set; }
    }
}
