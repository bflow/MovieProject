using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieProject.Models
{
    public partial class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> o) 
            : base(o) { Database.EnsureCreated(); }
        
        public DbSet<ProductData> ProductData { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<User> User { get; set; }
    }
}
