import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { ResolvedMovie } from '../models/resolvedMovie';
import { GetByIdEndPoint } from '@core/endpoints/movie/getbyid-endpoint/getbyid.endpoint';
import { catchError, map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class MovieResolver implements Resolve<ResolvedMovie> {

  constructor(
    private getmovieById: GetByIdEndPoint,
    private toastrService: ToastrService,
    private router: Router,
  ) { }

  // Operacija se izvrsava prije nego sto se ruta aktivira (resolve)
  resolve(route: ActivatedRouteSnapshot ): Observable<ResolvedMovie> {
    const id = route.paramMap.get('id'); // Dobavljamo iz url-a "id" i cuvamo ga u varijabli
    const param = route.paramMap.get('backurl');
    const backUrl = (param) ? param : ''; // Ako je param null bit ce prazan string u suprotnom ce se koristiti sacuvana vrijednost

    // Ako je vrijednost stvarno broj tjt +id korisnika preusmjerava na datu stranicu
    if (isNaN(+id)) {
      this.toastrService.error(`Incorrect format, id='${id}' should be a number`);
      this.router.navigateByUrl(backUrl);
      return of(null);
    }
    return this.getmovieById.getMovieById(+id).pipe( // Pipe se koristi za primjenu operacija na toku podataka
      // Ako hocemo samo da vratimo bas tu vrijednost koju dobijemo od map, a subscribe vracamo cijeli objekt
      map( // Map emitira transformiranu vrijednost kao rezultat
        movie => {
          if (movie) { // Ako film postoji vracamo objekat "movie"
            return {movie, backUrl};
          }
          this.toastrService.error(`Movie with id='${id}' was not found`);
          this.router.navigateByUrl(backUrl);
          return null;
        }
      ),
      catchError(
        error => {
          this.toastrService.error(`${error}`);
          this.router.navigateByUrl(backUrl);
          return of(null);
        }
      )
    );
  }
}
