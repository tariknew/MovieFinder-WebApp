import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MovieParameterService {
  filterByTitle = '';
  orderBy = 'Last Added';
  displayView = 'list';
}
