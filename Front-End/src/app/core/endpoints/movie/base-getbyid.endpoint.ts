import { Observable } from 'rxjs';
export interface BaseEndPoint<TNumber, TMovie> {
  getMovieById(movieID: TNumber): Observable<TMovie>;
}
