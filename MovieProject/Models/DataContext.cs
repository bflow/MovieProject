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
        
        public DbSet<ProductEvent> ProductData { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<User> User { get; set; }

        public void AddIncrementProductEvent(int eventType, List<TMDbLib.Objects.Search.SearchMovie> results = null, int movieId = 0)
        {
            if (eventType == 0)
            {
                foreach (var m in results)
                    AddIncrementEventByID(eventType, m.Id);
            }
            if (eventType == 1)
            {
                AddIncrementEventByID(eventType, movieId);
            }

            SaveChanges();
        }

        private void AddIncrementEventByID(int eventType, int movieID)
        {
            var productDataFound = ProductData.Find(movieID);

            if (productDataFound == null)
            {
                if (eventType == 0)
                    ProductData.Add(new ProductEvent { LocatorID = movieID, TotalSearchCount = 1 });
                if (eventType == 1)
                    ProductData.Add(new ProductEvent { LocatorID = movieID, TotalRentalCount = 1 });
            }
            else
            {
                if (eventType == 0)
                    productDataFound.TotalSearchCount++;
                if (eventType == 1)
                    productDataFound.TotalRentalCount++;
            }
        }

        public void AddUserEvent(int eventType, System.Security.Claims.ClaimsPrincipal user, string searchText = "", string searchResults = "", int movieID = 0)
        {
            if(eventType == 0)
            {
                UserEvents.Add(new UserEvent
                {
                        UserID = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value,
                        UserName = user.FindFirst(System.Security.Claims.ClaimTypes.Name).Value,
                        SearchTerms = searchText,
                        SearchResult = searchResults,
                        EventDate = System.DateTime.Now
                });
            }
            if (eventType == 1)
            {
                UserEvents.Add(new UserEvent
                {
                    UserID = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value,
                    UserName = user.FindFirst(System.Security.Claims.ClaimTypes.Name).Value,
                    MovieRental = movieID,
                    EventDate = System.DateTime.Now
                });
            }

            SaveChanges();
        }
    }
}
