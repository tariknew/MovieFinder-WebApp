import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from '../services/auth.services';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService,
              private router: Router) { }

  // canActivate je dio 'Guard' sistema
  canActivate(
    route: ActivatedRouteSnapshot,
    // 'UrlTree' vraca novu putanju koja ce se koristiti, a boolean da dobijemo informaciju je li korisnik logiran ili ne
    // Observable se koristi vecinom za asinhrone funkcije
    // Kada je potrebno nesto dohvatiti, prije izvrsenja neke druge radnje
    // On u ovom slucaju moze vratiti boolean ili usmjeriti na novi 'url'
    // 'Promise' -> 'kada aplikacija treba dohvatiti podatke s servera prije nego što se prikažu na zaslonu
    // Vecinom za login sistem se koristi 'Promise'
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.checkLoggedIn(state.url, route);
  }

  private checkLoggedIn(url: string, route: ActivatedRouteSnapshot): Observable<boolean> {
    return this.authService.getCurrentUser().pipe(
      map( user => {
        if (user) {
          // check if route is restricted by role
          if (route.data.roles && !route.data.roles.includes(user.role)) {
            // role not authorized so redirect to home page
            this.router.navigate(['/']);
            return false;
          }

          // authorized so return true
          return true;
        }

        // not logged in so redirect to login page
        this.authService.redirectUrl = url;
        this.router.navigate(['auth']);
        return false;
      })
    );
  }

}
