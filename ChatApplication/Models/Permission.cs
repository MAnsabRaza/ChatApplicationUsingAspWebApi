using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatApplication.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public DateTime current_date {  get; set; }
        public bool status {  get; set; }
        public int roleId {  get; set; }
        [ForeignKey("roleId")]
        public virtual Role Role { get; set; }
        public int moduleId {  get; set; }
        [ForeignKey("moduleId")]
        public virtual Module Module { get; set; }
    }
}