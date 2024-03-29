﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnWebBanHang.WebApp.Models
{
    public class ApplicationUserViewModel
    {
        public string Id { set; get; }
        public string FullName { set; get; }
        public DateTime BirthDay { set; get; }
        public string Bio { set; get; }
        public string Email { set; get; }
        public string PasswordHash { set; get; }
        public string UserName { set; get; }
        public string Address { get; set; }
        public string PhoneNumber { set; get; }
        public string VerificationToken { get; set; }
        public string PasswordVerificationToken { get; set; }
        public Nullable<DateTime> DateValidateToken { get; set; }
        public IEnumerable<ApplicationGroupViewModel> Groups { set; get; }
    }
}