﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONS.Core.Entities
{
   public class Users
    {
       [Key]
       public int UserId { get; set; }

       [Required]
       public string UserName { get; set; }

       [Required]
       public string Password { get; set; }

     

       [Required]
       public string UserType { get; set; }


    }
}
