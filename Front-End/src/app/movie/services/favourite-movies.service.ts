import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import {environment} from '@env/environment';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import {Movie} from '@core/models/view-models/movie';

@Injectable({
  providedIn: 'root'
})
export class FavoriteMoviesService {

  constructor(private http: HttpClient) { }

    addToFavorites(userId: number, movieId: number): Observable<Movie[]> {
      const apiUrl = `${environment.apiUrl}/FavouriteMovies/AddMovieToFavourites`;
      const requestBody = {userId, movieId}; // Skraćeni zapis
      return this.http.post<Movie[]>(apiUrl, requestBody).pipe(
        catchError(this.handleError)
      );
    }


  removeFromFavorites(userId: number, movieId: number): Observable<Movie[]> {
    const apiUrl = `${environment.apiUrl}/FavouriteMovies?userId=${userId}&movieId=${movieId}`;
    return this.http.delete<Movie[]>(apiUrl).pipe(
      catchError(this.handleError)
    );
  }

    getFavouriteMovies(userId: number): Observable<Movie[]> {
        const apiUrl = `${environment.apiUrl}/FavouriteMovies/GetFavouritesMovies?userId=${userId}`;
        return this.http.get<Movie[]>(apiUrl).pipe(
            catchError(this.handleError)
        );
    }


    isMovieFavourite(userId: number, movieId: number): Observable<boolean> {
    return this.http.get<boolean>(`${environment.apiUrl}/FavouriteMovies?userId=${userId}&movieId=${movieId}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(err: HttpErrorResponse) {
        let errorMessage: string;

        if (err.error instanceof ErrorEvent) {
            // A client-side or network error occurred. Handle it accordingly.
            errorMessage = 'An error occurred: ${err.error.message}';
        } else {
            // The backend returned an unsuccessful response code.
            // The response body may contain clues as to what went wrong,
            errorMessage = '${err.error}';
        }
        console.error(errorMessage);
        return throwError(errorMessage);
    }
}

