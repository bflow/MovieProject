using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieProject.Models;
using System.Collections.Generic;
using System.Linq;
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
        public string _userName = "";
        protected string _userID = "";
        TMDbClient _client = new TMDbClient("2f28d2662c4a95ba4f35cb6e136c98e4");
        private enum ProductEvents { Search, Rental };

        public MovieController(DataContext context)
        {
            _context = context;
        }

        // GET: api/movie/search/star
        [HttpGet("search/{searchText}")]
        public List<SearchMovie> SearchMovies(string searchText)
        {
            var results = _client.SearchMovieAsync(searchText, includeAdult: false).Result.Results;

            if (User != null && User.Identity.IsAuthenticated)
            {
                //_context.UserEvents.Add(new UserEvent {
                //    UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                //    UserName = User.FindFirst(ClaimTypes.Name).Value,
                //    SearchTerms = searchText,
                //    SearchResult = Newtonsoft.Json.JsonConvert.SerializeObject(results),
                //    EventDate = System.DateTime.Now
                //});

                _context.AddUserEvent((int)ProductEvents.Search, User, searchText, Newtonsoft.Json.JsonConvert.SerializeObject(results));
                //_context.SaveChanges();
            }

            //foreach (var m in results)
            //{
                //var productDataFound = _context.ProductData.Find(m.Id);
                //if (productDataFound == null)
                //    _context.ProductData.Add(new ProductEvent { LocatorID = m.Id, TotalSearchCount = 1 });
                //else
                //    productDataFound.TotalSearchCount++;

                _context.AddIncrementProductEvent((int)ProductEvents.Search, results);
            //}

            

            return results;
        }

        // GET: api/movie/search/1111
        [HttpGet("searchProduct/{locatorID}")]
        public ProductEvent GetProductDataByMovieID(int locatorID)
        {
            return _context.ProductData.Find(locatorID);
        }

        // GET api/user/events/bflow101
        [HttpGet("user/events/{userName}")]
        public IQueryable<UserEvent> GetUserEventsByUserName(string userName)
        {
            return _context.UserEvents.Where(e => e.UserName.Equals(userName));
        }

        [HttpPost("rent/{locatorID}")]
        public ActionResult RentMovie(int locatorID)
        {
            var results = _client.GetMovieAsync(locatorID);

            if (User != null && User.Identity.IsAuthenticated)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.AddUserEvent((int)ProductEvents.Rental, User, movieID: locatorID);
                        _context.AddIncrementProductEvent((int)ProductEvents.Rental, movieId: locatorID);
                    }
                    catch
                    {
                        transaction.Rollback();
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }

                return Ok("Enjoy your movie!");
            }

            return Unauthorized();
        }
    }   
}
