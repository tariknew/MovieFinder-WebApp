import { Component, Input, OnInit } from '@angular/core';
import { Movie } from '@core/models/view-models/movie';
import { noImg } from '@shared/helpers/noImgFound';
@Component({
  selector: 'app-movie-grid',
  templateUrl: './movie-grid.component.html',
  styleUrls: ['./movie-grid.component.css']
})
export class MovieGridComponent implements OnInit {

  movieData: Movie[];
  @Input() set movies(data: Movie[]) {
    this.movieData = data;
  }
  get movies(): Movie[] {
    return this.movieData;
  }
  getMovieImage(movieImage: any): any {
    if (movieImage) {
      return `data:image/jpeg;base64,${movieImage}`;
    } else {
      return noImg;
    }
  }
  constructor() { }

  ngOnInit() {
  }

}
