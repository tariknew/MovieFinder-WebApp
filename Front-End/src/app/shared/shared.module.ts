import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; // Moramo ovo dodati posto koristimo biblioteku
import { SelectInputComponent } from './components/select-input/select-input.component';
import { ScrollTopComponent } from './components/scroll-top/scroll-top.component';
import { PosterComponent } from './components/poster/poster.component';
import { CategoryInputComponent } from './components/category-input/category-input.component';
import { SearchComponent } from './components/search/search.component';
import { StarComponent } from './components/star/star.component';
import { StarRatingComponent } from './components/star-rating/star-rating.component';
import { ActorDialogComponent } from './components/actor-dialog/actor-dialog.component';
import { NotificationDialogComponent } from './components/notification-dialog/notification-dialog.component';
import { MaterialModule } from '@app/material.module';
@NgModule({
  declarations: [
    SpinnerComponent,
    SelectInputComponent,
    CategoryInputComponent,
    ScrollTopComponent,
    SearchComponent,
    StarRatingComponent,
    StarComponent,
    PosterComponent,
    ActorDialogComponent,
    NotificationDialogComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MaterialModule,
    ReactiveFormsModule
  ],
  exports: [
    CommonModule,
    SpinnerComponent,
    SelectInputComponent,
    ScrollTopComponent,
    CategoryInputComponent,
    SearchComponent,
    StarRatingComponent,
    StarComponent,
    PosterComponent,
    ActorDialogComponent,
    NotificationDialogComponent
  ],
  // Potrebno da se dialog prikaze (za dinamicko ucitavanje)
  entryComponents: [
    ActorDialogComponent,
    NotificationDialogComponent
  ]
})
export class SharedModule { }
