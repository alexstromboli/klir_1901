import { HttpClient } from '@angular/common/http';

export function FetchWithTimeout<T> (http: HttpClient, url: string, timeoutMs: number): Promise<T>
{
  var TimeIsUp = true;

  return new Promise<T> ((locResolve, locReject) =>
  {
    setTimeout(() => TimeIsUp && locReject (new Error ("timeout")), timeoutMs);

    http.get<T>(url)
      .subscribe(
        result => locResolve(result),
        error => locReject(error)
      );
  })
  .then(result =>
    {
      TimeIsUp = false;
      return result;
    })
  ;
}
