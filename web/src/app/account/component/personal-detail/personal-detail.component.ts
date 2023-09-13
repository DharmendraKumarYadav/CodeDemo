import { Profile } from './../../model/profile.model';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormGroupDirective, FormBuilder, FormControl, Validators } from '@angular/forms';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { Role } from 'src/app/shared/model/auth/role.model';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { AccountService } from 'src/app/shared/service/account.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { UserComponent } from 'src/app/user-management/components/user/user.component';
import { ValidateMobileNumber } from 'src/app/shared/validators/common-validator';

@Component({
  selector: 'app-personal-detail',
  templateUrl: './personal-detail.component.html',
  styleUrls: ['./personal-detail.component.scss']
})
export class PersonalDetailComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  formName: string = "My Profile"
  roles: Role[];
  constructor(private spinnerService: SpinnerService, private fb: FormBuilder,  private accountService: AccountService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.getProfileData();
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      fullName: new FormControl("", [Validators.required]),
      email: new FormControl("", [Validators.required,Validators.email]),
      userName: new FormControl("", [Validators.required]),
      phoneNumber: new FormControl("", [Validators.required,ValidateMobileNumber]),
      role: new FormControl("", [Validators.required]),

    });
  }
  populateFormData(formData: any) {
    this.formGroup.patchValue(formData);
  }
  get form() {
    return this.formGroup.controls;
  }

  get validationMessage() {
    return ValidationMessage.userProfileForm;
  }

  update() {
    if (this.formGroup.invalid) {
      return;
    }
    let formValue = this.formGroup.value;
    this.isLoading = true;
    this.spinnerService.start();
    this.accountService.updateCurrentUser(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Profile updated sucessfully.");
        this.isLoading = false;
        this.spinnerService.stop();
      },
      (exception: any) => {
        this.isLoading = false;
        this.spinnerService.stop();
        this.notificationService.showValidation(exception);
      }
    );
  }
  resteForm() {

  }

  getProfileData(){
    let spinerRef = this.spinnerService.start();
    this.accountService.getUser().subscribe(result => {
      this.populateFormData(result);
      this.spinnerService.stop();
    });
  }

}
