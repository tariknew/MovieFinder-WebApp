import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {BehaviorSubject, Observable, throwError} from 'rxjs';
import {environment} from '@env/environment';
import {ToastrService} from 'ngx-toastr';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {UserNotification} from '@core/models/view-models/usernotification';
import {catchError} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection;
  private notificationSubject$ = new BehaviorSubject<UserNotification[]>([]);

  // Ostalo
  userStorageData = null;
  lastNotification: any;
  previousNotificationLength = 0;
  currentNotificationLength = 0;

  constructor(
    private http: HttpClient,
    private toastrService: ToastrService
  ) {
  }

  startConnection() {
    const signalRUrl = `${environment.apiUrl}/notificationHub`;
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(signalRUrl)
      .build();

    this.hubConnection.start().then(() => {
      this.hubConnection.on('sendToUser', () => {
        this.loadNotifications(); // Ažuriraj notifikacije kada stigne nova notifikacija
      });
    });
  }

  showNotificationMessage(notification: string) {
    this.toastrService.success(`${this.lastNotification}`);
  }

  resetNotificationLength() {
    this.currentNotificationLength = 0;
    this.previousNotificationLength = 0;
  }

  loadNotifications(): void {
    const storedUserId = localStorage.getItem('userId');
    if (!storedUserId) {
      this.notificationSubject$.next([]); // Postavi prazan niz notifikacija
      return;
    } else {
      this.getUser();
      const endpoint = `${environment.apiUrl}/Notification?userId=${this.userStorageData.userId}`;
      this.http.get<UserNotification[]>(endpoint)
        .pipe(
          catchError(this.handleError)
        )
        .subscribe(
          notifications => {
            if (notifications.length > 0) {
              this.lastNotification = notifications[0].notificationText;
              this.currentNotificationLength = notifications.length;
              // Samo ako je 'Count' notifikacija povecan za +1 prikazi poruku
              // Ako je korisnik vec dobio pocetnu notifikaciju, na osvjezenju stranice mu se nece prikazati ista
              const firstNotificationDisplayed = localStorage.getItem('firstNotificationDisplayed');
              if (this.currentNotificationLength === this.previousNotificationLength + 1
                && this.previousNotificationLength > 0 || !firstNotificationDisplayed) {
                localStorage.setItem('firstNotificationDisplayed', 'true');
                this.showNotificationMessage(this.lastNotification);
              }
              this.previousNotificationLength = this.currentNotificationLength;
              this.notificationSubject$.next(notifications);
            } else {
              // Rijesen bug ako ostane 'jedna' notifikacija i nece da se ukloni iz dialog-a
              this.notificationSubject$.next([]);
              // Ako obrisemo sve notifikacije ili ostane jedna notifikacija -> brisemo iz local storage
              localStorage.removeItem('firstNotificationDisplayed');
            }
          }
        );
    }
  }

  updateNotification(notification: UserNotification) {
    const requestBody = {
      notificationId: notification.notificationId,
      userId: notification.userId,
      isRead: true
    };
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return this.http.put(`${environment.apiUrl}/Notification/UpdateNotification`, requestBody, httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  updateAllNotifications(userid: number) {
    return this.http.put(`${environment.apiUrl}/Notification/UpdateAllNotifications?userId=${userid}`, {})
      .pipe(
        catchError(this.handleError)
      );
  }

  getNotification(): Observable<UserNotification[]> {
    return this.notificationSubject$.asObservable();
  }

  getUser() {
    const storedUserId = localStorage.getItem('userId');
    this.userStorageData = {
      userId: JSON.parse(storedUserId)
    };
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
}
