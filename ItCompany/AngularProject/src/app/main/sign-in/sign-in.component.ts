import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { UserService } from '../../services/user.service';

import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { AuthStatus } from '../../models/enums/AuthStatus';
import { FormValidatorService } from '../../services/form-validator.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {
  public errorMessage: string;
  public signinForm: FormGroup;

  private authStatus: AuthStatus;
  public successMessage: string;

  get isShowLoader(): boolean {
    return this.authStatus == AuthStatus.Submited;
  }

  constructor(private userService: UserService, private route: ActivatedRoute, private formBuilder: FormBuilder, private formService: FormValidatorService) { }

  ngOnInit() {
    this.route
      .queryParams
      .subscribe(params => {
        let success = params['success'] == "true";
        if (success) {
          this.successMessage = "User account created, let's try our service!"
        }
      });
    this.signinForm = this.getValidators();
  }

  OnSubmit({ value, valid }) {
    if (this.signinForm.invalid) {
      this.formService.validateAllFormFields(this.signinForm);
      return;
    }
    this.authStatus = AuthStatus.Submited;
    this.userService.userAuthentication(value.username, value.password,
      (data) => {
        this.authStatus = AuthStatus.GotResponse;
      },
      (errorResponse) => {
        this.authStatus = AuthStatus.GotResponse;
        this.errorMessage = errorResponse.message;
      });
  }

  displayFieldValidationClass(field: string) {
    let control = this.signinForm.get(field);
    if (control && (control.dirty || control.touched)) {
      return control.valid ? 'form-control-success' : 'form-control-danger';
    }

    return '';
  }

  displayFieldGroupValidationClass(field: string) {
    let control = this.signinForm.get(field);
    if (control && (control.dirty || control.touched)) {
      return control.valid ? 'has-success' : 'has-error';
    }

    return '';
  }

  private getValidators() {
    return this.formBuilder.group({
      username: ['', [
        Validators.required
      ]],
      password: ['', [
        Validators.required
      ]]
    });
  }

}
