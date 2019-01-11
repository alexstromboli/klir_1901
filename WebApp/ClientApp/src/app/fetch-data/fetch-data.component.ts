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
    var timeoutMs = 6000;

    FetchWithTimeout<WeatherForecast[]> (
        http,
        baseUrl + 'api/SampleData/WeatherForecasts',
        timeoutMs
      )
      .then (result => this.forecasts = result)
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
