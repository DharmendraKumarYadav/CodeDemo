import { ChangePassword } from './../../model/chnage-password.model';
import { Component, OnInit, ViewChild} from '@angular/core';
import { FormGroup, FormBuilder, FormGroupDirective, FormControl, Validators } from '@angular/forms';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ComparePassword } from 'src/app/shared/validators/compare-password-validator';
import { WhiteSapceValidator } from 'src/app/shared/validators/common-validator';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { AccountService } from 'src/app/shared/service/account.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';

@Component({
  selector: 'app-chnage-password',
  templateUrl: './chnage-password.component.html',
  styleUrls: ['./chnage-password.component.scss']
})
export class ChnagePasswordComponent implements OnInit {
  hideCurrentPassword = true;
  hidePassword = true;
  hideConfirmPassword = true;
  loading: boolean;
  mobnumPattern = "^[7-9][0-9]{9}$";
  chnagePasswordForm: FormGroup;
  submitted = false;
  returnUrl: string;
  subscribe: any;

  changePassword = new ChangePassword();
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  constructor(private spinnerService: SpinnerService,private formBuilder: FormBuilder, private authService: AuthenticationService, private notificationService: NotificationService,private accountService:AccountService) {

  }

  ngOnInit() {
    this.changePasswordForms();
  }
  changePasswordForms() {
    this.chnagePasswordForm = this.formBuilder.group({
      currentPassword: new FormControl('', [
        Validators.required
      ]),
      newPassword: new FormControl('', [
        Validators.required, Validators.minLength(5), Validators.maxLength(15),WhiteSapceValidator
      ]),
      passwordConfirm: new FormControl('', [
        Validators.required
      ]),
    },
      { validators: ComparePassword("newPassword", "passwordConfirm") }
    );
  }
  get f() { return this.chnagePasswordForm.controls; }
  update() {
    if (this.chnagePasswordForm.invalid) {
      return;
    }
    this.changePassword = this.chnagePasswordForm.value;
    this.spinnerService.start();
    this.accountService.updatePassword(this.changePassword)
      .subscribe(
        () => {
          this.spinnerService.stop();
          this.notificationService.showSuccess("Password Changed successfully.")
          this.authService.logout();

        },
        (exception: any) => {
          this.notificationService.openSnackBar(exception);
          this.spinnerService.stop();
        });
  }

  resteForm() {
    this.formGroupDirective.resetForm();
  }
  get form() { return this.chnagePasswordForm.controls; }

  get validationMessage() {
    return ValidationMessage.changePasswordForm;
  }
}
