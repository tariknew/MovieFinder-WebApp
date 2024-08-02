import {Component, ChangeDetectorRef, OnDestroy, OnInit} from '@angular/core';
import {MediaMatcher} from '@angular/cdk/layout';
import {AuthService} from 'app/auth/services/auth.services';
import {Router} from '@angular/router';
import {Role} from '@core/models/auth/role';
import {SignalRService} from '@core/services/signalr.service';
import {MatDialog} from '@angular/material/dialog';
import {NotificationDialogComponent} from '@shared/components/notification-dialog/notification-dialog.component';
import {UserNotification} from '@core/models/view-models/usernotification';
import {JwtService} from '@app/auth/services/jwt.services';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit, OnDestroy {
  constructor(
    private changeDetectionRef: ChangeDetectorRef,
    private authService: AuthService,
    private jwtService: JwtService,
    private router: Router,
    private dialog: MatDialog,
    private signalRService: SignalRService,
    media: MediaMatcher
  ) {
    this.mobileQuery = media.matchMedia('(max-width: 880px)');
    this.mobileQueryListener = () => {
      changeDetectionRef.detectChanges();
    };
    this.mobileQuery.addEventListener('change', this.mobileQueryListener);
  }

  pageTitle = 'MovieShop';
  mobileQuery: MediaQueryList;
  notifications: UserNotification[] = [];
  private mobileQueryListener: () => void;

  user$ = this.authService.getCurrentUser();

  logOut(): void {
    this.authService.logout();
    this.router.navigate(['/welcome']);
  }

  ngOnDestroy(): void {
    this.mobileQuery.removeEventListener('change', this.mobileQueryListener);
  }

  isAdmin(): boolean {
    const storedToken = localStorage.getItem('token');
    if (storedToken) {
      const decodedToken = this.jwtService.getInfoFromToken(storedToken);
      const roleFromToken = decodedToken.role;
      return roleFromToken && roleFromToken.includes(Role.Admin);
    }
  }

  ngOnInit(): void {
    this.getUserNotifications();
  }

  openDialog(notificationData: any): void {
    const dialogRef = this.dialog.open(NotificationDialogComponent, {
      data: notificationData
    });
  }

  getUserNotifications(): void {
    this.signalRService.getNotification().subscribe(
      notifications => {
        this.notifications = notifications;
      }
    );
  }
}
