import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthStatus } from '../../models/enums/AuthStatus';
import { UserService } from '../../services/user.service';
import { PasswordValidation } from '../../validators/password.validation';
import { FormValidatorService } from '../../services/form-validator.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  public errorMessage: string;
  public signupForm: FormGroup;
  private authStatus: AuthStatus;

  get isShowLoader(): boolean {
    return this.authStatus == AuthStatus.Submited;
  }

  constructor(private userService: UserService, private router: Router, private formBuilder: FormBuilder, private formService: FormValidatorService) { }

  ngOnInit() {
    this.signupForm = this.getValidators();
  }

  OnSubmit({ value, valid }) {
    if (this.signupForm.invalid) {
      this.formService.validateAllFormFields(this.signupForm);
      return;
    }
    this.authStatus = AuthStatus.Submited
    this.userService.registerUser(value,
      (data) => {
        this.authStatus = AuthStatus.GotResponse;
        this.errorMessage = "";
        this.signupForm.reset();
        this.router.navigate(["/signin"], { queryParams: { success: true } })
      },
      (errorResponse) => {
        this.authStatus = AuthStatus.GotResponse;
        if (typeof errorResponse.error == 'string') {
          this.errorMessage = errorResponse.error;
        } else
          if (typeof errorResponse.error == 'object' && errorResponse.status == 400) {
            this.errorMessage = "";
            for (let element in errorResponse.error) {
              this.errorMessage += `${errorResponse.error[element]}<br>`
            };
          }
          else {
            this.errorMessage = errorResponse.message;
          }
      });
  }

  displayFieldValidationClass(field: string) {
    let control = this.signupForm.get(field);
    if (control && (control.dirty || control.touched)) {
      return control.valid ? 'form-control-success' : 'form-control-danger';
    }

    return '';
  }

  displayFieldGroupValidationClass(field: string) {
    let control = this.signupForm.get(field);
    if (control && (control.dirty || control.touched)) {
      return control.valid ? 'has-success' : 'has-error';
    }

    return '';
  }

  private getValidators() {
    return this.formBuilder.group({
      email: ['', [
        Validators.required, Validators.email
      ]],
      password: ['', [
        Validators.required
      ]],
      confirmPassword: ['', [
        Validators.required,
      ]]
    }, {
        validator: PasswordValidation.matchPassword
      });
  }
}
