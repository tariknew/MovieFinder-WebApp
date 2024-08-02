import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@app/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProfileRoutingModule } from './profile-routing.module';
import { ProfileEditInfoComponent } from '@app/user-profile/components/profile-edit-info/profile-edit-info.component';
@NgModule({
  declarations: [
    ProfileEditInfoComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ProfileRoutingModule,
    ReactiveFormsModule
  ],
})
export class ProfileModule { }
