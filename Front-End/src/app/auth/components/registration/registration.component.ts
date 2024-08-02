import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '@auth/services/auth.services';
import { Router } from '@angular/router';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  registrationForm: FormGroup;
  loading = false;
  registrationStatus: string;
  errorMessage: string;
  hidePassword = true;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router, private toastrService: ToastrService) {}

  ngOnInit(): void {
    this.registrationForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required],
      acceptTerms: [false, Validators.requiredTrue]
    }, { validator: this.passwordsMatchValidator });
  }

  onSubmit(): void {
    if (this.registrationForm.valid && this.registrationForm.get('acceptTerms').value) {
      this.loading = true;
      const userData = this.registrationForm.value;

      this.authService.register(userData).subscribe(
        (response) => {
          this.loading = false;
          this.registrationStatus = 'success';
          this.toastrService.success('Your account was created successfully!', `Hello ${userData.username}`);
          this.router.navigate(['/auth/login']);
        },
        (error) => {
          this.loading = false;
          this.registrationStatus = 'error';
          console.error('Error:', error);

          if (error === 'Username already exists') {
            this.errorMessage = 'Username already exists. Please choose another one.';
            this.registrationForm.get('username').setErrors({userNameExists: true});
          } else if (error === 'Email already exists') {
            this.errorMessage = 'Email already exists. Please use a different one.';
            this.registrationForm.get('email').setErrors({emailExists: true});
          } else {
            this.errorMessage = 'Registration failed. Please try again.';
          }

          console.log('ErrorMessage:', this.errorMessage);
        }
      );
    }
    }




  passwordsMatchValidator(form: FormGroup) {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');

    return password && confirmPassword && password.value !== confirmPassword.value
      ? { passwordsMismatch: true }
      : null;
  }

  togglePasswordVisibility(input: HTMLInputElement): void {
    this.hidePassword = !this.hidePassword;
    input.type = this.hidePassword ? 'password' : 'text';
  }

  openTermsDialog(): void {
  }
}
