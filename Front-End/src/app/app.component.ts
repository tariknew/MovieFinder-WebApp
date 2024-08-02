import {Component} from '@angular/core';
import {Router, Event, NavigationStart, NavigationEnd, NavigationError, NavigationCancel} from '@angular/router';
import {SignalRService} from '@core/services/signalr.service';
import {AuthService} from '@auth/services/auth.services';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  loading = true;

  constructor(private router: Router, private signalRService: SignalRService, private authService: AuthService) {
    router.events.subscribe((routerEvent: Event) => {
      this.checkRouterEvent(routerEvent);
    });
    // Uspostavljamo glavnu 'SignalR' konekciju
    this.signalRService.startConnection();
    // Ako osvjezimo stranicu potrebno je opet ucitati notifikacije
    this.signalRService.loadNotifications();
  }

  checkRouterEvent(routerEvent: Event): void {
    if (routerEvent instanceof NavigationStart) {
      this.loading = true;
    }
    if (routerEvent instanceof NavigationEnd ||
      routerEvent instanceof NavigationCancel ||
      routerEvent instanceof NavigationError) {
      this.loading = false;
    }
  }
}
