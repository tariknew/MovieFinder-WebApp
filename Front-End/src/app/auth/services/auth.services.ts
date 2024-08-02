import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {BehaviorSubject, Observable, throwError} from 'rxjs';
import {catchError, tap} from 'rxjs/operators';
import {environment} from '@env/environment';
import {AuthResponse1, AuthResponse2} from 'app/core/models/auth/authResponse';
import {LocalStorageService} from '@core/services/local-storage.service';
import {LoginFormData} from 'app/core/models/auth/authData';
import {RegisterFormData} from 'app/core/models/auth/authData';
import {SignalRService} from '@core/services/signalr.service';
import {TokenResponse} from '@core/models/auth/tokenResponse';
import {JwtService} from '@app/auth/services/jwt.services';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private redirectUrlink: string;
  userData = null;

  // Postavlja se (BehaviorSubject) koji čuva trenutno stanje korisnika
  private user$ = new BehaviorSubject<AuthResponse2>(null);

  constructor(
    private http: HttpClient,
    private localStorageService: LocalStorageService,
    private signalRService: SignalRService,
    private jwtService: JwtService
  ) {
    this.initializeUser();
  }

  login(form: LoginFormData): Observable<AuthResponse1> {
    const apiUrl = `${environment.apiUrl}/AuthManagement/Login`;
    return this.http.post(`${apiUrl}?UserName=${form.username}&Password=${form.password}`, form, this.headers)
      .pipe(
        // Tap se cesto koristi za izvrsavanje drugih operacija, a narucito za login i postavljanje header-a
        // Tap je koristan operator za obavljanje sporednih efekata tijekom obrade observabla, a da pritom ne mijenja sam tok podataka.
        tap((response: AuthResponse1) => {
          const userData = {
            userId: response.userId,
            token: response.token,
            refreshToken: response.refreshToken
          };
          this.setData('userId', response.userId);
          this.setData('token', response.token);
          this.setData('refreshToken', response.refreshToken);
          // Da bi se odmah poslje login-a ucitali podaci
          this.initializeUser();
        }),
        catchError(this.handleError)
      );
  }

  // Saljemo podatke o korisniku na sve observable
  // Ovo koristimo u slucaju osvježenja stranice da jos uvijek imamo taj objekt
  // Potrebno za 'UserName' polje
  initializeUser(): void {
    const storedToken = localStorage.getItem('token');
    if (storedToken) {
      const decodedToken = this.jwtService.getInfoFromToken(storedToken);
      const usernameFromToken = decodedToken.username;
      const user = {
        userName: usernameFromToken
      };
      // Ovo znaci da se postavlja response u u BehaviorSubject.
      this.user$.next(user);
    }
  }

  // GET Metodom dobavimo trenutne podatke od korisnika koje su pohranjene u objekat
  // Ali moramo imati 'initializeUser' funkciju da bi se azuriralo svaki put
  getCurrentUser(): Observable<any | null> {
    return this.user$.asObservable();
  }

  private setData(key: string, token: any): void {
    this.localStorageService.setItem(key, token);
  }

  private get headers() {
    return {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    };
  }

  logout(): void {
    this.localStorageService.removeItem('userId');
    this.localStorageService.removeItem('token');
    this.localStorageService.removeItem('refreshToken');
    // Ako ne postoji 'userId' u storage-u vrati ce prazan niz (osigurano)
    this.signalRService.loadNotifications();
    this.user$.next(null);
  }

  get redirectUrl(): string {
    return this.redirectUrlink;
  }

  set redirectUrl(url: string) {
    this.redirectUrlink = url;
  }

  getUser() {
    const storedToken = localStorage.getItem('token');
    const storedRefreshToken = localStorage.getItem('refreshToken');
    if (storedToken && storedRefreshToken) {
      const decodedToken = this.jwtService.getInfoFromToken(storedToken);
      const usernameFromToken = decodedToken.username;
      this.userData = {
        userName: usernameFromToken,
        refreshToken: JSON.parse(storedRefreshToken)
      };
    }
  }

  // Observable se koristi za asinhrono pracenje podataka
  // Kada se izvrsi operacija mi mozemo da obradimo 'TokenResponse'
  refreshToken(): Observable<TokenResponse> {
    // Dobavi korisnicke podatke iz localStorage
    this.getUser();
    const requestBody = {
      userName: this.userData.userName,
      refreshToken: this.userData.refreshToken
    };
    return this.http.post<TokenResponse>(`${environment.apiUrl}/AuthManagement/RefreshToken`, requestBody, {
      headers: {
        'Content-Type': 'application/json'
      }
    }).pipe(
      tap((response: TokenResponse) => {
        const accessToken = response.accessToken;
        const refreshToken = response.refreshToken;
        // Postavljamo novi accessToken i refreshToken u localStorage
        this.setData('token', accessToken);
        this.setData('refreshToken', refreshToken);
      }),
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
      errorMessage = `${err.error}`;
    }
    console.error(err);
    return throwError(errorMessage);
  }

  register(form: RegisterFormData): Observable<boolean> {
    const apiUrl = `${environment.apiUrl}/AuthManagement/Register`;
    const finalApiUrl = `${apiUrl}?UserName=${form.username}&Password=${form.password}&Email=${form.email}`;
    const requestBody = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return this.http.post<boolean>(finalApiUrl, requestBody).pipe(
      catchError(this.handleError)
    );
  }

}
