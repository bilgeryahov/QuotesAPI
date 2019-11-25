using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesAPI.Data;
using QuotesAPI.Models;

namespace QuotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuoteController : ControllerBase
    {
        private QuoteDbContext _quoteDbContext;

        public QuoteController(QuoteDbContext quoteDbContext)
        {
            _quoteDbContext = quoteDbContext; 
        }

        // GET: api/Quote
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IActionResult Get(string sort)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            IQueryable<Quote> quotes;
            switch (sort)
            {
                case "desc":
                    quotes = _quoteDbContext.Quotes.OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes = _quoteDbContext.Quotes.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = _quoteDbContext.Quotes;
                    break;
            }
            quotes = quotes.Where(q => q.UserId == userId);
            return Ok(quotes);
        }

        [HttpGet("[action]")]
        public IActionResult PagingQuote(int? pageNumber, int? pageSize)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            IEnumerable<Quote> quotes = _quoteDbContext.Quotes;
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 2;

            quotes = quotes.Where(q => q.UserId == userId);

            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public IActionResult SearchQuote(string type)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return Ok(_quoteDbContext.Quotes.Where(q => q.Type.StartsWith(type)).Where(q => q.UserId == userId));
        }

        // GET: api/Quote/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            Quote quote = _quoteDbContext.Quotes.Find(id);

            if (userId != quote.UserId)
            {
                return BadRequest();
            }

            if (quote == null)
            {
                return NotFound("No recourd found against this id...");
            }

            return Ok(quote);
        }

        // POST: api/Quote
        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            quote.UserId = userId;
            _quoteDbContext.Quotes.Add(quote);
            _quoteDbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT: api/Quote/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            Quote quoteToChange = _quoteDbContext.Quotes.Find(id);

            if (userId != quoteToChange.UserId)
            {
                return BadRequest();
            }

            if (quoteToChange == null)
            {
                return NotFound("No recourd found against this id...");
            }

            quoteToChange.Title = quote.Title;
            quoteToChange.Author = quote.Author;
            quoteToChange.Description = quote.Description;
            quoteToChange.Type = quote.Type;
            quoteToChange.CreatedAt = quote.CreatedAt;
            _quoteDbContext.SaveChanges();

            return Ok("Updated successfully");
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            Quote quoteToDelete = _quoteDbContext.Quotes.Find(id);

            if (userId != quoteToDelete.UserId)
            {
                return Unauthorized("Sorry, you cannot delete this record...");
            }

            if (quoteToDelete == null)
            {
                return NotFound("No recourd found against this id...");
            }

            _quoteDbContext.Quotes.Remove(quoteToDelete);
            _quoteDbContext.SaveChanges();

            return Ok("Quote deleted...");
        }
    }
}
