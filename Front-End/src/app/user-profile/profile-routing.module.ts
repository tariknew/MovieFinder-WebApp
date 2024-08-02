import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProfileEditInfoComponent } from '@app/user-profile/components/profile-edit-info/profile-edit-info.component';

const movieRoutes: Routes = [
  { path: 'user', component: ProfileEditInfoComponent }
];
@NgModule({
  imports: [RouterModule.forChild(movieRoutes)],
  exports: [RouterModule]
})
export class ProfileRoutingModule { }
