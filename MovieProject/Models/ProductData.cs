using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieProject.Models
{
    public class ProductData
    {
        public int ID { get; set; }
        [Key]
        public int LocatorID { get; set; }
        public int TotalSearchCount { get; set; }
        public int TotalRentalCount { get; set; }
    }
}
