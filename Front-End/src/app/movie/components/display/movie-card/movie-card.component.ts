import { Component, OnInit, Input } from '@angular/core';
import { Movie } from '@core/models/view-models/movie';
import { noImg } from '@shared/helpers/noImgFound';

@Component({
  selector: 'app-movie-card',
  templateUrl: './movie-card.component.html',
  styleUrls: ['./movie-card.component.scss']
})
export class MovieCardComponent implements OnInit {

  @Input() movie: Movie;
  constructor() { }

  getMovieImage(movieImage: any): any {
    if (movieImage) {
      return `data:image/jpeg;base64,${movieImage}`;
    } else {
      return noImg;
    }
  }

  ngOnInit() {
  }
}
