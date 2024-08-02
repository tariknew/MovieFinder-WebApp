import { BaseEndPoint } from '../base-getall.endpoint';
import { Movie } from '@core/models/view-models/movie';
import { SearchObject } from '@core/models/requests/searchObject';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { environment } from '@env/environment';
@Injectable({
  providedIn: 'root'
})

export class GetAllEndPoint implements BaseEndPoint<SearchObject, Movie[]> {
  private getMoviesUrl = `${environment.apiUrl}/Movie`;
  private getMoviesCountUrl = `${environment.apiUrl}/Movie/GetMoviesCount`;
  constructor(private httpClient: HttpClient) {
  }
  getMovies(searchObject?: SearchObject): Observable<Movie[]> {
    let params: any = { PageNumber: searchObject.pagenumber };
    const areAllParamsNull =
      searchObject.movietitle === null &&
      searchObject.pricefrom == null &&
      searchObject.priceto == null &&
      searchObject.categoryid === null &&
      searchObject.sortedcolumn === null &&
      searchObject.isdescending === null;
    // ...params -> stvara kopiju trenutnih svojstava objekta
    // Title: title -> dodaje novo svojstvo Title s vrijednoscu title u trenutnu kopiju objekta
    // Cijeli rezultat se dodjeljuje natrag varijabli 'params' zamjenjujući prethodnu vrijednost
    // Ovo je korisno jer očuva originalni objekt params, a istovremeno omogućuje dodavanje novih svojstava
    // Filtering
    if (!areAllParamsNull) {
      if (searchObject.movietitle != null) {
        params = {...params, Title: searchObject.movietitle};
      }

      if (searchObject.pricefrom != null) {
        params = {...params, PriceFrom: searchObject.pricefrom};
      }

      if (searchObject.priceto != null) {
        params = {...params, PriceTo: searchObject.priceto};
      }

      if (searchObject.categoryid != null) {
        params = {...params, CategoryID: searchObject.categoryid};
      }

      if (searchObject.sortedcolumn != null) {
        params = {...params, SortedColumn: searchObject.sortedcolumn};
      }

      if (searchObject.isdescending != null) {
        params = {...params, IsDescending: searchObject.isdescending};
      }
    }
    const queryParams = this.buildQueryString(params);
    const url = `${this.getMoviesUrl}?${queryParams}`;

    return this.httpClient.get<Movie[]>(url).pipe(
      catchError(this.handleError)
    );
  }
  getMoviesCount(searchObject?: SearchObject): Observable<number> {
    let params: any = { };
    // Filtering
    if (searchObject.movietitle != null) {
        params = {...params, Title: searchObject.movietitle};
      }

    if (searchObject.categoryid != null) {
        params = {...params, CategoryID: searchObject.categoryid};
      }

    if (searchObject.pricefrom != null) {
        params = {...params, PriceFrom: searchObject.pricefrom};
      }

    if (searchObject.priceto != null) {
        params = {...params, PriceTo: searchObject.priceto};
      }

    if (searchObject.sortedcolumn != null) {
        params = {...params, SortedColumn: searchObject.sortedcolumn};
      }

    if (searchObject.isdescending != null) {
        params = {...params, IsDescending: searchObject.isdescending};
      }
    const queryParams = this.buildQueryString(params);
    const url = `${this.getMoviesCountUrl}?${queryParams}`;

    return this.httpClient.get<number>(url).pipe(
      catchError(this.handleError)
    );
  }
  private buildQueryString(params: any): string {
    // Vraca niz kljuceva iz objekta params
    // Npr ako imamo params objekt { Title: 'Film1', CategoryID: 1 } on ce vratiti niz ['Title', 'CategoryID']
    return Object.keys(params)
      // Ova metoda prolazi kroz svaki kljuc u nizu i stvara niz stringova
      // Svaki string ima oblik 'kljuc=vrijednost'
      // Kljuc i vrijednost se uzimaju iz objekta params
      // Na kraju dobijemo [ 'Title=Film1', 'CategoryID=1' ]
      .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(params[key])}`)
      // Konacno ova metoda spaja sve stringove koristeci znak '&'
      // Ako params objekt sadrzi samo jedan kljuc 'join' i dodavanje znaka '&' se nece primjeniti
      .join('&');
  }
  private handleError(err: HttpErrorResponse) {
    let errorMessage: string;
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Backend returned code: ${err.status}`;
    }
    console.error(err);
    return throwError(errorMessage);
  }
}


