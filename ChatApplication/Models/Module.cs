using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApplication.Models
{
    public class Module
    {
        public int Id { get; set; }
        public DateTime current_date {  get; set; }
        public string module_icon {  get; set; }
        public string module_name { get; set; }
        public string href {  get; set; }
    }
}