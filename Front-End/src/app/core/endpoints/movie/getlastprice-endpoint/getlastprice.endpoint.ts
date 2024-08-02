import { BaseEndPoint } from '../base-getlastprice.endpoint';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { environment } from '@env/environment';
@Injectable({
  providedIn: 'root'
})

export class GetLastPriceEndPoint implements BaseEndPoint<void, number> {
  constructor(private httpClient: HttpClient) {
  }
  getMovieLastPrice(request: void): Observable<number> {
    const url = `${environment.apiUrl}/Movie/GetMovieLastPrice`;
    return this.httpClient.get<number>(url)
      .pipe(
        // tap(data => console.log(data)),
        catchError(this.handleError)
      );
  }
  private handleError(err: any) {
    let errorMessage: string;
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Backend returned code ${err.status}: ${err.body.error}`;
    }
    console.error(err);
    return throwError(errorMessage);
  }
}


