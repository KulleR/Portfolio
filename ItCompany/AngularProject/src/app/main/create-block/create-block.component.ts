import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { FormValidatorService } from '../../services/form-validator.service';
import { ScriptBlockService } from '../../services/script-block.service';
import { UserService } from '../../services/user.service';
import { ScriptBlockForUpdate } from '../../models/script/ScriptBlockForUpdate';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-block',
  templateUrl: './create-block.component.html',
  styleUrls: ['./create-block.component.css']
})
export class CreateBlockComponent implements OnInit {

  public createBlockForm: FormGroup;

  constructor(private formService: FormValidatorService, private formBuilder: FormBuilder,
    private scriptBlockService: ScriptBlockService, private userService: UserService,
    private router: Router) { }

  ngOnInit() {
    this.createBlockForm = this.getValidators();
    this.onUpdatedUserClaims(() => {
      
    });
  }

  OnSubmit({ value, valid }) {
    if (this.createBlockForm.invalid) {
      this.formService.validateAllFormFields(this.createBlockForm);
      return;
    }

    let scriptBlock = new ScriptBlockForUpdate();
    scriptBlock.Text = value.text;
    scriptBlock.Header = value.header;

    this.scriptBlockService.create(this.userService.currentUser.scriptId, scriptBlock).subscribe(() => {
      this.router.navigateByUrl(`/`);
    });
  }

  displayFieldValidationClass(field: string) {
    let control = this.createBlockForm.get(field);
    if (control && (control.dirty || control.touched)) {
      return control.valid ? 'form-control-success' : 'form-control-danger';
    }

    return '';
  }

  displayFieldGroupValidationClass(field: string) {
    let control = this.createBlockForm.get(field);
    if (control && (control.dirty || control.touched)) {
      return control.valid ? 'has-success' : 'has-error';
    }

    return '';
  }

  private getValidators() {
    return this.formBuilder.group({
      header: ['', [
        Validators.required
      ]],
      text: ['', [
        Validators.required
      ]]
    });
  }


  onUpdatedUserClaims(callback) {
    this.userService.updateUserClaims().subscribe(user => {
      this.userService.currentUser = user;
      callback();
    })
  }
}
