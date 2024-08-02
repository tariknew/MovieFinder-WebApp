import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MovieResolver } from '@core/services/movie.resolver';
import { MovieSelectComponent } from './components/display/movie-select/movie-select.component';
import { MovieDetailComponent } from './components/display/movie-detail/movie-detail.component';
import {MovieListFavouritesComponent} from '@app/movie/components/display/movie-list-favourites/movie-list-favourites.component';

const movieRoutes: Routes = [
  { path: '', component: MovieSelectComponent },
  {
    path: 'Favourites',
    component: MovieListFavouritesComponent
  },
  {
    path: ':id',
    resolve: { movie: MovieResolver },
    component: MovieDetailComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(movieRoutes)],
  exports: [RouterModule]
})
export class MovieRoutingModule { }
