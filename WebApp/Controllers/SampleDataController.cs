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

        private enum FaultyResultType
        {
            Ok = 0,
            Long,
            TooLong,
            Error500,
            Garbage
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<WeatherForecast>> WeatherForecasts()
        {
            var rng = new Random();

            FaultyResultType Occasion = (FaultyResultType)(rng.Next(Enum.GetNames (typeof(FaultyResultType)).Length));

            switch (Occasion)
            {
                case FaultyResultType.Ok:
                    // okay
                    return Ok (Enumerable.Range(1, 6).Select(index => new WeatherForecast
                    {
                        DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                        TemperatureC = rng.Next(-20, 55),
                        Summary = Summaries[rng.Next(Summaries.Length)]
                    }));

                case FaultyResultType.Long:
                    // long
                    System.Threading.Thread.Sleep (4000);
                    goto case FaultyResultType.Ok;

                case FaultyResultType.TooLong:
                    // way too long
                    System.Threading.Thread.Sleep (10000);
                    goto case FaultyResultType.Ok;

                case FaultyResultType.Error500:
                    // server fault
                    return StatusCode (500);

                case FaultyResultType.Garbage:
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
