using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnWebBanHang.WebApp.Models
{
    public class PostTagViewModel
    {
        public int PostID { set; get; }

        public string TagID { set; get; }

        public virtual PostViewModel Post { set; get; }

        public virtual TagViewModel Tag { set; get; }
    }
}