import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { mustMatch } from '@shared/validators/must-match.validator';
import { ForgotPassService } from '../../services/forgotpassword.service';
import { LocalStorageService } from '@core/services/local-storage.service';
import { first } from 'rxjs/operators';
@Component({
  selector: 'app-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.css']
})
export class RestartComponent implements OnInit {
  resetForm: FormGroup;
  emailValue = '';
  tokenValue: '';
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private localStorageService: LocalStorageService,
    private toastr: ToastrService,
    private forgotpassService: ForgotPassService
  ) {
  }
  ngOnInit() {
    this.getTokenFromRoute();
    this.createForm();
  }

  get storedRecoveryEmail(): string {
    return this.emailValue = this.localStorageService.getItem('email') || '';
  }
  private createForm(): void {
    this.resetForm = this.formBuilder.group({
        token: [this.tokenValue],
        email: [this.storedRecoveryEmail],
        newPassword: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required]
      }, {
        validator: mustMatch('newPassword', 'confirmPassword')
      });
  }
  private getTokenFromRoute(): void {
    this.route.queryParams.subscribe(params => {
      this.tokenValue = params.token;
    });
  }
  onSubmit() {
    // stop here if form is invalid
    if (this.resetForm.invalid) {
      return;
    }
    this.toastr.clear(); // Trenutna obavještenja se brisu (npr. neki errori)
    this.forgotpassService.resetPassword(this.resetForm.value)
      // First uzme samo prvu vrijednost (to jest objekt ovaj this.resetForm.value) i nako sto ga primi subscribe
      // Ce se zavrsiti. Ako imamo vise  manipulacija poslje, one ce biti ignorisane.
      .pipe(first())
      .subscribe(
        () => {
          this.toastr.success('Password has been reset successfully!');
          this.forgotpassService.deleteSavedData(); // Obrisi sacuvani email iz header-a
          // Ako postoji definisan url, onda ce ga preusmjeriti tu u suprotnom na pocetnu stranicu ga preusmjerava
          if (this.forgotpassService.redirectUrl) {
            this.router.navigateByUrl(this.forgotpassService.redirectUrl);
          } else {
            this.router.navigate(['/']);
          }
        },
        error => {
          this.toastr.error(error);
        }
      );
  }
}
