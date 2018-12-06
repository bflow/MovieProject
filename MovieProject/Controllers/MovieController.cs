using Microsoft.AspNetCore.Mvc;
using MovieProject.Models;
using System.Collections.Generic;
using System.Linq;
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
        [HttpGet("search/{searchText}/{userId}")]
        public ActionResult<List<SearchMovie>> SearchMovies(string searchText, int userId)
        {
            TMDbClient client = new TMDbClient(_apikey);
            var results = client.SearchMovieAsync(searchText).Result.Results;
            
            var user = _context.UserData.Find(userId);
            if (user != null)
            {
                _context.UserEvents.Add(new UserEvent { UserID = userId, SearchTerms = searchText, SearchResult = Newtonsoft.Json.JsonConvert.SerializeObject(results), EventDate = System.DateTime.Now });
                _context.SaveChanges();
            }

            return results;
        }

        // GET api/movie/5
        [HttpGet("{id}")]
        public ActionResult<TMDbLib.Objects.Movies.Movie> GetMovieByID(int id)
        {
            TMDbClient client = new TMDbClient(_apikey);
            return client.GetMovieAsync(id).Result;
        }

        // GET api/user/5
        [HttpGet("user/{id}")]
        public ActionResult<User> GetUserByID(int id)
        {
            User u = _context.User.Find(id);
            return u;
        }

        // GET api/user/events/101
        [HttpGet("user/events/{id}")]
        public IQueryable<UserEvent> GetUserEventsByUserID(int id)
        {
            var events = _context.UserEvents.Where(e => e.UserID == id);
            return events;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
