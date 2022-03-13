using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models
{
    public class SigninRequest
    {
        [Required]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Sifre")]
        public string Password { get; set; }
        [Display(Name = "BeniHatırla")]

        public bool IsRemember { get; set; }
    }
}
