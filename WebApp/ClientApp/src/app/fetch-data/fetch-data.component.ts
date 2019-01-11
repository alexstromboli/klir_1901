import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];
  public error: any;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {
    var TimeoutMs = 6000;
    var TimeIsUp = true;

    new Promise<WeatherForecast[]> ((resolve, reject) =>
    {
      setTimeout(() => TimeIsUp && reject (new Error ("timeout")), TimeoutMs);

      http.get<WeatherForecast[]>(baseUrl + 'api/SampleData/WeatherForecasts')
        .subscribe(
          result => resolve(result),
          error => reject(error)
        );
    })
    .then(result =>
      {
        TimeIsUp = false;
        this.forecasts = result;
      })
    .catch (error => console.error(error))
    ;
  }
}

interface WeatherForecast {
  dateFormatted: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
