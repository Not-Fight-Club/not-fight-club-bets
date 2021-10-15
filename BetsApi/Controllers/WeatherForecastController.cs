using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BetsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _iConfig;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration iConfig)
        {
            _logger = logger;
            _iConfig = iConfig;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("FEtoAPI")]
        public string GetString1() { return "response from bets"; }


        static readonly HttpClient client = new HttpClient();
        
        [HttpGet("apiToapi")]
        public async Task<string> GetString()
        {
            //calling the character api
            HttpResponseMessage response = await client.GetAsync(_iConfig["Character:url"]+":5005/api/character/testing");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;

        }

        [HttpGet("dbtest")]
        public string dbTest()
        {
            
            string query = "select Inventory from Product where Id = @productId";
            using (SqlConnection conn = new SqlConnection("Server=tcp:cafe-sqldb-server.database.windows.net,1433;Initial Catalog=Cafe-sqldb;Persist Security Info=False;User ID=cafe-server-admin;Password=Robo$solo;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@productId", "B00Q7JESL0");
                conn.Open();
                int stock = (int)cmd.ExecuteScalar();
                return stock.ToString();
            }
        }
        
    
    }
}
