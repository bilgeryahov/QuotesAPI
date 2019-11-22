using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesAPI.Data;
using QuotesAPI.Models;

namespace QuotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private QuoteDbContext _quoteDbContext;

        public QuoteController(QuoteDbContext quoteDbContext)
        {
            _quoteDbContext = quoteDbContext; 
        }

        // GET: api/Quote
        [HttpGet]
        public IActionResult Get(string sort)
        {
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
            return Ok(quotes);
        }

        [HttpGet("[action]")]
        public IActionResult PagingQuote(int? pageNumber, int? pageSize)
        {
            IEnumerable<Quote> quotes = _quoteDbContext.Quotes;
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 2;

            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        // GET: api/Quote/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            Quote quote = _quoteDbContext.Quotes.Find(id);

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
            _quoteDbContext.Quotes.Add(quote);
            _quoteDbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT: api/Quote/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
            Quote quoteToChange = _quoteDbContext.Quotes.Find(id);

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
            Quote quoteToDelete = _quoteDbContext.Quotes.Find(id);

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
