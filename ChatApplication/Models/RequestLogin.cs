using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApplication.Models
{
    public class RequestLogin
    {
        public string email { get;set; }
        public string password { get;set; }
    }
}