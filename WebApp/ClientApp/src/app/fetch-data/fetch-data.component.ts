import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FetchWithTimeout } from '../fetchwithtimeout';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];
  public error: any;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {
    this.TryFetchData(http, baseUrl);
  }

  TryFetchData(http: HttpClient, baseUrl: string): void
  {
    var timeoutMs = 6000;

    FetchWithTimeout<WeatherForecast[]> (
        http,
        baseUrl + 'api/SampleData/WeatherForecasts',
        timeoutMs
      )
      .then (result =>
        {
          this.forecasts = result;
          this.error = null;
        })
      .catch (error =>
        {
          console.error(error);
          this.forecasts = null;
          this.error = error;

          setTimeout(() =>
          {
            this.TryFetchData(http, baseUrl);
          }, 3000);
        })
      ;
  }
}

interface WeatherForecast {
  dateFormatted: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
