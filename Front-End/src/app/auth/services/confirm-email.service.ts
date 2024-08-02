import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class ConfirmEmailService {
    constructor(private http: HttpClient) {}
    confirmEmail(userName: string, token: string): Observable<any> {
        const apiUrl = `${environment.apiUrl}/AuthManagement/ConfirmEmail?userName=${userName}&token=${token}`;
        return this.http.get(apiUrl).pipe(
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
