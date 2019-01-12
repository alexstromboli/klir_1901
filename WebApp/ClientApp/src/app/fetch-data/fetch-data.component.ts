import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FetchWithTimeout } from '../fetchwithtimeout';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent
{
  http: HttpClient;
  baseUrl: string;

  public forecasts: WeatherForecast[];
  public error: any;
  public retryTime: Date;
  public secondsToRetry: number;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {
    this.http = http;
    this.baseUrl = baseUrl;

    this.TryFetchData();
  }

  TryFetchData(): void
  {
    var timeoutMs = 6000;

    FetchWithTimeout<WeatherForecast[]> (
        this.http,
        this.baseUrl + 'api/SampleData/WeatherForecasts',
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

          this.retryTime = new Date();
          this.retryTime.setSeconds(this.retryTime.getSeconds() + 5);

          setTimeout(() => this.WaitingForRetry(), 0);
        })
      ;
  }

  WaitingForRetry()
  {
    var dt = new Date();

    if (dt >= this.retryTime)
    {
      this.retryTime = null;
      this.secondsToRetry = null;
      this.error = null;

      setTimeout(() => this.TryFetchData(), 0);

      return;
    }

    this.secondsToRetry = Math.floor((<any>this.retryTime - <any>dt) / 1000);

    setTimeout(() => this.WaitingForRetry(), 1000);
  }
}

interface WeatherForecast {
  dateFormatted: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
