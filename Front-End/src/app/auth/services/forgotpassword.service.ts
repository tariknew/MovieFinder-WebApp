import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { RestartpasswordFormData } from '@core/models/auth/authData';
import { Observable, throwError } from 'rxjs';
import { ForgotPasswordResponse } from '@core/models/auth/forgotpwResponse';
import { LocalStorageService } from '@core/services/local-storage.service';
import { catchError, tap } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class ForgotPassService {
  private redirectUrlink: string;
  constructor(private http: HttpClient,
              private localStorageService: LocalStorageService) { }
  recoveryMail(email: string): Observable<ForgotPasswordResponse> {
    const apiUrl = `${environment.apiUrl}/AuthManagement/ForgetPassword`;
    return this.http.post(`${apiUrl}?email=${email}`, {email}, this.headers)
      .pipe(
        tap((response: ForgotPasswordResponse) => {
          this.setToken('email', response.email);
        }),
        catchError(this.handleError)
      );
  }
  private setToken(email: string, response: string): void {
    this.localStorageService.setItem(email, response);
  }
  private get headers() {
    return {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
  }

  deleteSavedData(): void {
    this.localStorageService.removeItem('email');
  }
  resetPassword(form: RestartpasswordFormData) {
    const apiUrl = `${environment.apiUrl}/AuthManagement/ResetPassword?Token=${form.token}&Email=${form.email}`;
    return this.http.post(`${apiUrl}&NewPassword=${form.newPassword}&ConfirmPassword=${form.confirmPassword}`, form).pipe(
      catchError(this.handleError)
    );
  }

  get redirectUrl(): string {
    return this.redirectUrlink;
  }

  set redirectUrl(url: string) {
    this.redirectUrlink = url;
  }
  private handleError(err: HttpErrorResponse) {
    let errorMessage: string;
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `${err.error.message}`;
    }
    console.error(err);
    return throwError(errorMessage);
  }
}
