using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Service.Models
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool EmailSent { get; set; }
        public string newPassword { get; set; }
        public Nullable<DateTime> DateValidateToken { get; set; }
    }
}
