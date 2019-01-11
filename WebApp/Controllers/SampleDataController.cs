using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<WeatherForecast>> WeatherForecasts()
        {
            var rng = new Random();

            int Occasion = rng.Next(21) % 5;

            switch (Occasion)
            {
                case 0:
                    // long
                    System.Threading.Thread.Sleep (4000);
                    goto case 2;

                case 1:
                    // way too long
                    System.Threading.Thread.Sleep (10000);
                    goto case 2;

                case 2:
                    // okay
                    return Ok (Enumerable.Range(1, 6).Select(index => new WeatherForecast
                    {
                        DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                        TemperatureC = rng.Next(-20, 55),
                        Summary = Summaries[rng.Next(Summaries.Length)]
                    }));

                case 3:
                    // server fault
                    return StatusCode (500);

                case 4:
                    // garbage response
                    return StatusCode (200, "Qb5VfjkQ9dPVj42dprhN");
            }

            throw new Exception ("Faulty math");
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
