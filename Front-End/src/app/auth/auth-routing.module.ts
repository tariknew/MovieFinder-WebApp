import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ForgotComponent } from './components/forgotpassword/forgot.component';
import { RestartComponent } from './components/resetpassword/reset.component';
import { AuthPageComponent } from './components/auth-page/auth-page.component';
import {RegistrationComponent} from '@auth/components/registration/registration.component';
import {ConfirmEmailComponent} from '@auth/components/confirm-email/confirm-email.component';

const authRoutes: Routes = [
  {
    path: '',
    component: AuthPageComponent,
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'forgotPass', component: ForgotComponent },
      { path: 'restartPass', component: RestartComponent },
      { path: 'register', component: RegistrationComponent },
      {path: 'confirmEmail', component: ConfirmEmailComponent }



]
  }
];
@NgModule({
  imports: [RouterModule.forChild(authRoutes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
