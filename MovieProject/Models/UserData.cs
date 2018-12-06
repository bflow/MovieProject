using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieProject.Models
{
    public class UserData
    {
        [Key]
        public int ID { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool LoggedIn { get; set; }
        //[InverseProperty("UserID")]
        //public List<UserEvent> Events { get; set; }
    }
}
