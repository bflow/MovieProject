using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieProject.Models
{
    public class UserEvent
    {
        public int ID { get; set; }
        public string SearchTerms { get; set; }
        public string SearchResult { get; set; }
        public DateTime EventDate { get; set; }
        public int MovieRental { get; set; }
        public int UserID { get; set; }
    }
}