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
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Quote
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Quote/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
