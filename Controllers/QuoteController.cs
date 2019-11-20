using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QuotesAPI.Models;

namespace QuotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private static List<Quote> Quotes { get; set; } = new List<Quote>()
        {
            new Quote() {Id = 0, Author = "Bilger", Description = "The brain is wider than the sky.", Title = "Inspirational" },
            new Quote() {Id = 1, Author = "Ilker", Description = "The love stories never have endings.", Title = "Love Stories" }
        };

        [HttpGet]
        public IEnumerable<Quote> Get()
        {
            return Quotes;
        }
        
        [HttpPost]
        public void Post([FromBody]Quote quote)
        {
            Quotes.Add(quote);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Quote quote)
        {
            Quotes[id] = quote;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Quotes.RemoveAt(id);
        }
    }
}