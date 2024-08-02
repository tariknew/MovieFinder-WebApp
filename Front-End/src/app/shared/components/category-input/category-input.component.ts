import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormControl } from '@angular/forms';
import { distinctUntilChanged } from 'rxjs/operators';
import { Category } from '@core/models/view-models/category';

@Component({
  selector: 'app-category-input',
  templateUrl: './category-input.component.html',
  styleUrls: ['./category-input.component.css']
})
export class CategoryInputComponent implements OnInit {

  selectControl = new FormControl();
  @Input() options: Category[] = [];
  @Input() label: string;
  @Input() set option(value: Category[]) {
   this.selectControl.setValue(value);
  }
  @Output() optionChange = new EventEmitter<number>();

  constructor() { }

  ngOnInit() {
    this.selectControl.valueChanges.pipe(
      distinctUntilChanged((prev, curr) => prev === curr),
    ).subscribe(
      value => this.optionChange.emit(value)
    );
  }

}
