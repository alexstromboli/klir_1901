using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApi.SQLite;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Enforce database
            using (var db = new WeatherForecastContext())
            {
                db.Database.EnsureCreated();

                if (db.WeatherForecasts.Count() == 0)
                {
                    string[] Summaries = new []
                    {
                        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                    };

                    var rng = new Random();

                    for (int i = 1; i <= 20; ++i)
                    {
                        WeatherForecast wf = new WeatherForecast
                        {
                            Id = i,
                            Date = DateTime.Today.AddDays(i),
                            TemperatureC = rng.Next(-20, 55),
                            Summary = Summaries[rng.Next(Summaries.Length)]
                        };

                        db.WeatherForecasts.Add(wf);
                    }

                    db.SaveChanges();
                }
            }
            #endregion

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
