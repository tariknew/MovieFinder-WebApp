import { BaseEndPoint } from '../base-getallcategories.endpoint';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { environment } from '@env/environment';
import { Category } from '@core/models/view-models/category';
@Injectable({
  providedIn: 'root'
})

export class GetAllCategoriesEndPoint implements BaseEndPoint<void, Category[]> {
  constructor(private httpClient: HttpClient) {
  }
  getAllCategories(request: void): Observable<Category[]> {
    const url = `${environment.apiUrl}/Category`;
    return this.httpClient.get<Category[]>(url).pipe(
      catchError(this.handleError)
    );
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


