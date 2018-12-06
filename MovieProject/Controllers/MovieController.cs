using Microsoft.AspNetCore.Mvc;
using MovieProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Search;

namespace MovieProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private DataContext _context;
        private readonly string _apikey = "2f28d2662c4a95ba4f35cb6e136c98e4";
        protected string _returnString;

        public MovieController(DataContext context)
        {
            _context = context;
        }

        // GET: api/movie/search/star
        [HttpGet("search/{searchText}")]
        public ActionResult<List<SearchMovie>> SearchMovies(string searchText)
        {
            TMDbClient client = new TMDbClient(_apikey);
            var results = client.SearchMovieAsync(searchText).Result.Results;
            var userName = "";
            var userID = "";

            if (User != null && User.Identity.IsAuthenticated)
            {
                userName = User.FindFirst(ClaimTypes.Name).Value;
                userID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.UserEvents.Add(new UserEvent { UserID = userID, UserName = userName, SearchTerms = searchText, SearchResult = Newtonsoft.Json.JsonConvert.SerializeObject(results), EventDate = System.DateTime.Now });
                _context.SaveChanges();
            }

            foreach(var m in results)
            {
                var productDataFound = _context.ProductData.Find(m.Id);
                if (productDataFound == null)
                    _context.ProductData.Add(new ProductData { LocatorID = m.Id, TotalSearchCount = 1 });
                else
                    productDataFound.TotalSearchCount++;
            }

            _context.SaveChanges();

            return results;
        }

        // GET: api/movie/search/1111
        [HttpGet("searchProduct/{locatorID}")]
        public ActionResult<ProductData> GetProductDataByID(int locatorID)
        {
            var results = _context.ProductData.Find(locatorID);
            return results;
        }

        // GET api/movie/5
        [HttpGet("{id}")]
        public ActionResult<TMDbLib.Objects.Movies.Movie> GetMovieByID(int id)
        {
            TMDbClient client = new TMDbClient(_apikey);
            return client.GetMovieAsync(id).Result;
        }

        // GET api/user/events/bflow101
        [HttpGet("user/events/{userName}")]
        public IQueryable<UserEvent> GetUserEventsByUserName(string userName)
        {
            var events = _context.UserEvents.Where(e => e.UserName.Equals(userName));
            return events;
        }
        
        [HttpPost("rent/{locatorID}")]
        public async Task<IActionResult> RentMovie(int locatorID)
        {
            TMDbClient client = new TMDbClient(_apikey);
            var results = new List<TMDbLib.Objects.Movies.Movie>();
            var userName = "";
            var userID = "";

            if (User != null && User.Identity.IsAuthenticated)
            {
                userName = User.FindFirst(ClaimTypes.Name).Value;
                userID = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                results.Add(await client.GetMovieAsync(locatorID));

                foreach(var m in results)
                {
                    _context.UserEvents.Add(new UserEvent { UserID = userID, UserName = userName, MovieRental = m.Id, EventDate = System.DateTime.Now });

                    var productDataFound = _context.ProductData.Find(m.Id);
                    if (productDataFound == null)
                        _context.ProductData.Add(new ProductData { LocatorID = m.Id, TotalRentalCount = 1 });
                    else
                        productDataFound.TotalRentalCount++;
                }
                _context.SaveChanges();
                return Ok("Enjoy your movie!");
            }

            return Unauthorized();
        }
    }
}
