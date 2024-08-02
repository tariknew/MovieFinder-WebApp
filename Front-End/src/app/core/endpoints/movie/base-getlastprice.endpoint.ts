import { Observable } from 'rxjs';
export interface BaseEndPoint<TRequest, TNumber> {
  getMovieLastPrice(request: TRequest): Observable<TNumber>;
}
