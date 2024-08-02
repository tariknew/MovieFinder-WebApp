import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ForgotPassService } from '../../services/forgotpassword.service';
@Component({
  selector: 'app-forgot',
  templateUrl: './forgot.component.html',
  styleUrls: ['./forgot.component.css']
})
export class ForgotComponent implements OnInit {

  forgotForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private forgotpassService: ForgotPassService
  ) {
    this.createForm();
  }

  ngOnInit() {
  }

  private createForm() {
    this.forgotForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit() {
    // stop here if form is invalid
    if (this.forgotForm.invalid) {
      return;
    }

    this.forgotpassService.recoveryMail(this.forgotForm.controls.email.value)
      .subscribe(
        responce => this.toastr.success(`Recovery instructions sent at ${this.forgotForm.controls.email.value}`),
        error => this.toastr.error(error)
      );
  }
}
