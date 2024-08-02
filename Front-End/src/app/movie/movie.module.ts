import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@app/material.module';
import { SharedModule } from '@shared/shared.module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { MovieSelectComponent } from './components/display/movie-select/movie-select.component';
import { MovieRoutingModule } from './movie-routing.module';
import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { MovieCardComponent } from './components/display/movie-card/movie-card.component';
import { MovieGridComponent } from './components/display/movie-grid/movie-grid.component';
import { MovieDetailComponent } from './components/display/movie-detail/movie-detail.component';
import { MovieListComponent } from './components/display/movie-list/movie-list.component';
import {MovieListFavouritesComponent} from '@app/movie/components/display/movie-list-favourites/movie-list-favourites.component';

@NgModule({
  declarations: [
    MovieSelectComponent,
    MovieCardComponent,
    MovieListComponent,
    MovieGridComponent,
    MovieDetailComponent,
    MovieListFavouritesComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    SharedModule,
    FormsModule,
    NgxSliderModule,
    MovieRoutingModule,
    ReactiveFormsModule
  ],
})
export class MovieModule { }
