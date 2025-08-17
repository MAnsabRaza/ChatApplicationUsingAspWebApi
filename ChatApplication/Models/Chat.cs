using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatApplication.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public DateTime current_date {  get; set; }
        public int userId {  get; set; }
        [ForeignKey("userId")]
        public virtual User User { get; set; }
        public Guid sessionId {  get; set; }
        public string message {  get; set; }
        public string response {  get; set; }
    }
}