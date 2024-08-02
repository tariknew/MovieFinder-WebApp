import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'mat-star-rating',
  templateUrl: './star-rating.component.html',
  styleUrls: ['./star-rating.component.css'],
  encapsulation: ViewEncapsulation.Emulated
})
export class StarRatingComponent implements OnInit {

  // tslint:disable-next-line:no-input-rename
  @Input('rating') protected rating = 3;
  // tslint:disable-next-line:no-input-rename
  @Input('starCount') protected starCount = 5;
  // tslint:disable-next-line:no-input-rename
  @Input('color') protected color = 'accent';
  @Output() private ratingUpdated = new EventEmitter();

  protected ratingArr = [];
  constructor() {
  }
  ngOnInit() {
    for (let index = 0; index < this.starCount; index++) {
      this.ratingArr.push(index);
    }
  }
  onClick(rating: number) {
    this.ratingUpdated.emit(rating);
    return false;
  }
  showIcon(index: number) {
    if (this.rating >= index + 1) {
      return 'star';
    } else {
      return 'star_border';
    }
  }
}
export enum StarRatingColor {
  primary = 'primary',
  accent = 'accent',
  warn = 'warn'
}
