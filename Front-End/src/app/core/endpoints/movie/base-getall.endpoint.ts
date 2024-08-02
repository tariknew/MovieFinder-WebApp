import { Observable } from 'rxjs';
export interface BaseEndPoint<SearchObject, Movie> {
  getMovies(searchObject: SearchObject): Observable<Movie>;
}
