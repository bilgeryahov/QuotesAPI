using System.Collections.Generic;
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
        public IEnumerable<Quote> Get()
        {
            return _quoteDbContext.Quotes;
        }

        // GET: api/Quote/5
        [HttpGet("{id}", Name = "Get")]
        public Quote Get(int id)
        {
            return _quoteDbContext.Quotes.Find(id);
        }

        // POST: api/Quote
        [HttpPost]
        public void Post([FromBody] Quote quote)
        {
            _quoteDbContext.Quotes.Add(quote);
            _quoteDbContext.SaveChanges();
        }

        // PUT: api/Quote/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Quote quote)
        {
            Quote quoteToChange = _quoteDbContext.Quotes.Find(id);
            quoteToChange.Title = quote.Title;
            quoteToChange.Author = quote.Author;
            quoteToChange.Description = quote.Description;
            _quoteDbContext.SaveChanges();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Quote quoteToDelete = _quoteDbContext.Quotes.Find(id);
            _quoteDbContext.Quotes.Remove(quoteToDelete);
            _quoteDbContext.SaveChanges();
        }
    }
}
