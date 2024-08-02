import { BaseEndPoint } from '../base-getbyid.endpoint';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { environment } from '@env/environment';
import { Movie } from '@core/models/view-models/movie';
@Injectable({
  providedIn: 'root'
})

export class GetByIdEndPoint implements BaseEndPoint<number, Movie> {
  constructor(private httpClient: HttpClient) {
  }
  private handleError(err: HttpErrorResponse) {
    let errorMessage: string;

    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `${err.error}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }
  getMovieById(movieId: number): Observable<Movie> {
    const url = `${environment.apiUrl}/Movie/${movieId}`;
    return this.httpClient.get<Movie>(url).pipe(
      catchError(this.handleError)
    );
  }
}


