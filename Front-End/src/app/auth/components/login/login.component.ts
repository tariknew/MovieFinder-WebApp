import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.services';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { SignalRService } from '@core/services/signalr.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  loading = false;
  hidePassword = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private toastr: ToastrService,
    private signalRService: SignalRService,
    private authService: AuthService
  ) {
    this.createForm();
  }
  ngOnInit() { }
  private createForm() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }
  get form() {
    return this.loginForm.controls;
  }
  onSubmit() {
    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }
    this.toastr.clear();
    this.loading = true;
    this.authService.login(this.loginForm.value)
      .pipe(first())
      .subscribe(
        () => {
          this.loading = false;
          this.toastr.success('Logged in successfully!');
          // Potrebno pozvati kako bi se trenutno ucitale notifikacije
          this.signalRService.loadNotifications();
          if (this.authService.redirectUrl) {
            this.router.navigateByUrl(this.authService.redirectUrl);
          } else {
            this.router.navigate(['/']);
          }
        },
        error => {
          this.toastr.error(error);
          this.loading = false;
        }
      );
  }
}
