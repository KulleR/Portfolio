import { Component, OnInit, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-show-input-errors',
  templateUrl: './show-input-errors.component.html',
  styleUrls: ['./show-input-errors.component.css']
})
export class ShowInputErrorsComponent implements OnInit {

  constructor() { }

  private static readonly errorMessages = {
    'required': () => 'Field is required',
    'email': (params) => "Invalid e-mail format",
    'matchPassword': (params) => "Passwords do not match",
    'minlength': (params) => 'The min number of characters is ' + params.requiredLength,
    'maxlength': (params) => 'The max allowed number of characters is ' + params.requiredLength,
    'pattern': (params) => 'The required pattern is: ' + params.requiredPattern,
    'date': (params) => "Date must be in '1980/10/25' format",
    'digits': (params) => "This field must be a number"
  };

  ngOnInit(): void {

  }

  @Input()
  private control: FormControl;

  shouldShowErrors(): boolean {
    return this.control &&
      this.control.errors &&
      (this.control.dirty || this.control.touched);
  }

  listOfErrors(): string[] {
    return Object.keys(this.control.errors)
      .map(field => this.getMessage(field, this.control.errors[field]));
  }

  private getMessage(type: string, params: any) {
    if (type in ShowInputErrorsComponent.errorMessages){
      return ShowInputErrorsComponent.errorMessages[type](params);
    }
    return '';
  }

}
