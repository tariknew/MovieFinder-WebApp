import { Component, OnInit } from '@angular/core';
import { FavoriteMoviesService } from '@app/movie/services/favourite-movies.service';
import { Movie } from '@core/models/view-models/movie';
import { FormBuilder, FormGroup } from '@angular/forms';
import { noImg } from '@shared/helpers/noImgFound';
import {ToastrService} from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
    selector: 'app-movie-list-favourites',
    templateUrl: './movie-list-favourites.component.html',
    styleUrls: ['./movie-list-favourites.component.css']
})
export class MovieListFavouritesComponent implements OnInit {
    favoriteMovies: Movie[] = [];
    userData = null;
    reviewForm: FormGroup;
    isFavorite = false;
    showRemoveButton = false;

    constructor(
        private favoriteMoviesService: FavoriteMoviesService,
        private formBuilder: FormBuilder,
        private toastrService: ToastrService,
        private router: Router
    ) {}

    ngOnInit() {
      this.getUser();
      if (this.userData.userId) {
        this.getFavoriteMovies();
      }

    }

  getUser() {
    const storedUserId = localStorage.getItem('userId');
    if (storedUserId) {
      this.userData = {
        userId: JSON.parse(storedUserId),
      };
      if (this.userData) {
        this.createForm();
      }
    }
  }
    getMovieImage(movieImage: any): any {
        if (movieImage) {
            return `data:image/jpeg;base64,${movieImage}`;
        } else {
            return noImg;
        }
    }

    private createForm(): void {
        this.reviewForm = this.formBuilder.group({
            inputComment: [''],
        });
    }
    getFavoriteMovies(): void {
        this.getUser();
        this.favoriteMoviesService.getFavouriteMovies(this.userData.userId).subscribe(
            (movies: Movie[]) => {
                this.favoriteMovies = movies;
            },
            (error) => this.toastrService.error(error),
        );
    }


    removeFromFavorites(movieId: number): void {
        this.favoriteMoviesService.removeFromFavorites(this.userData.userId, movieId).subscribe(
            () => {
                this.toastrService.success('The movie has been successfully removed from favorites');
                this.isFavorite = false; // Ažuriraj status da film nije omiljen
                this.getFavoriteMovies();
                this.router.navigate(['/movies/Favourites']);
            },
            (error) => this.toastrService.error(error),
        );
    }


}

