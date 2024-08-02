import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';
import { MaterialModule } from '../material.module';
import { AuthRoutingModule } from './auth-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { ForgotComponent } from './components/forgotpassword/forgot.component';
import { RestartComponent } from './components/resetpassword/reset.component';
import { AuthPageComponent } from './components/auth-page/auth-page.component';
import { HttpClientModule } from '@angular/common/http';
import {RegistrationComponent} from '@auth/components/registration/registration.component';
import {ConfirmEmailComponent} from '@auth/components/confirm-email/confirm-email.component';

@NgModule({
  declarations: [
    LoginComponent,
    AuthPageComponent,
    ForgotComponent,
    RestartComponent,
    RegistrationComponent,
    ConfirmEmailComponent

  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
})
export class AuthModule { }
