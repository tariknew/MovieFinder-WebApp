import { Observable } from 'rxjs';
export interface BaseEndPoint<Request, Category> {
  getAllCategories(request: Request): Observable<Category>;
}
