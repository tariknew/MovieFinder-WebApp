import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {UserNotification} from '@core/models/view-models/usernotification';
import {SignalRService} from '@core/services/signalr.service';
import {Router} from '@angular/router';
import {DOCUMENT} from '@angular/common';

@Component({
  selector: 'app-notification-dialog',
  templateUrl: './notification-dialog.component.html',
  styleUrls: ['./notification-dialog.component.css']
})
export class NotificationDialogComponent implements OnInit {
  userStorageData: any;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: UserNotification[],
    @Inject(DOCUMENT) private document: Document,
    private signalRService: SignalRService,
    public dialogRef: MatDialogRef<UserNotification>,
    private router: Router,
  ) {
  }

  ngOnInit(): void {
    // Osiguravamo da ukoliko je upaljen dialog, a dodana je notifikacija trenutno.
    // Dialog ce se azurirat
    this.loadNotifications();
  }

  redirectToMovie(movieId: number): void {
    // Vraca konstruisani URL
    const movieRoute = this.router.createUrlTree(['/movies', movieId]).toString();
    this.dialogRef.close();
    this.router.navigate(['/movies', movieId]).then(() => {
      window.location.href = movieRoute;
    });
  }

  loadNotifications(): void {
    this.signalRService.loadNotifications();
    this.signalRService.getNotification().subscribe(notifications => {
      this.data = notifications;
    });
  }

  markAsRead(notification: UserNotification): void {
    this.signalRService.updateNotification(notification).subscribe(() => {
      this.signalRService.loadNotifications();
      this.signalRService.getNotification().subscribe(notifications => {
        // Ažuriranje lokalne kopije podataka u komponenti
        this.data = notifications;
      });
    });
  }

  markAllAsRead(): void {
    this.getUser();
    this.signalRService.updateAllNotifications(this.userStorageData.userId).subscribe(() => {
      this.signalRService.loadNotifications();
      this.signalRService.getNotification().subscribe(notifications => {
        // Ažuriranje lokalne kopije podataka u komponenti
        this.data = notifications;
      });
    });
  }

  getUser() {
    const storedUserId = localStorage.getItem('userId');
    this.userStorageData = {
      userId: JSON.parse(storedUserId)
    };
  }
}
