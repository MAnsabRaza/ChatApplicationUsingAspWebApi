using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone_number { get; set; }
        public string address { get; set; }
    }
}