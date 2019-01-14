using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApi.SQLite
{
    public class WeatherForecastContext : DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=weatherforecasts.db");
        }
    }

    public class WeatherForecast
    {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }
    }
}
