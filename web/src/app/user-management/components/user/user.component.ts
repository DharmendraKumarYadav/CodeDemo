import { ValidateMobileNumber } from 'src/app/shared/validators/common-validator';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
import { Component, Inject, OnInit } from '@angular/core';
import { AccountService } from 'src/app/shared/service/account.service';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { Role } from 'src/app/shared/model/auth/role.model';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { UserAction } from 'src/app/shared/enums/table-action.enum';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  formName: string = "Create User"
  roles: Role[];
  isEdit: boolean = false;
  isAdd: boolean = false;
  isView: boolean = false;
  dealers:any;
  constructor(@Inject(MAT_DIALOG_DATA) public modalData: ModalData, private spinnerService: SpinnerService, private fb: FormBuilder, public dialogRef: MatDialogRef<UserComponent>, private accountService: AccountService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.getRoles();
    this.checkFormAction(this.modalData);
    this.getdealers();
  }
  checkFormAction(data: ModalData) {
    if (data.action == UserAction.Edit) {
      this.isEdit = true;
      this.formName="Edit User"
      this.populateFormData(data.data);
    } else if (data.action == UserAction.Add) {
      this.formName="Create User"
      this.isAdd = true;
    }
    else if (data.action == UserAction.View) {
      this.formName="View User"
      this.populateFormData(data.data);
      this.isView = true;
      this.formGroup.disable();
    }
    this.updateValidation();
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      fullName: new FormControl("", [Validators.required]),
      email: new FormControl("", [Validators.required,Validators.email]),
      password: new FormControl("", [Validators.required]),
      userName: new FormControl("", [Validators.required]),
      phoneNumber: new FormControl("", [Validators.required,ValidateMobileNumber]),
      role: new FormControl("", [Validators.required]),
      dealerIds: new FormControl([]),
      isEnabled: new FormControl(true),
    });
  }
  populateFormData(formData: any) {
    this.formGroup.patchValue(formData);
  }
  updateValidation() {
    if (this.isAdd) {
      this.formGroup.get('password').clearValidators();
      this.formGroup.get('password').updateValueAndValidity();
    } else {
      this.formGroup.controls["password"].clearValidators();
      this.formGroup.controls["password"].updateValueAndValidity();

    }
    this.updateBrokerValidation();
  }
  getdealers(){
    this.accountService.getDealers().subscribe(result => {
      this.dealers = result;
    });

  }
updateBrokerValidation(){
  if(this.isBroker){
    this.formGroup.get('dealerIds').setValidators(Validators.required);;
    this.formGroup.get('dealerIds').updateValueAndValidity();
  }else{
    this.formGroup.get('dealerIds').clearValidators();
    this.formGroup.get('dealerIds').updateValueAndValidity();
  }
}
get isBroker():boolean{
  return this.formGroup.controls["role"].value=="broker"?true:false;
}

  get form() {
    return this.formGroup.controls;
  }

  get validationMessage() {
    return ValidationMessage.createUserForm;
  }
  save() {
    if (this.formGroup.invalid) {
      return;
    }
    let formValue = this.formGroup.value;
    this.isLoading = true;
    this.spinnerService.start();
    this.accountService.createUser(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("User created sucessfully.");
        this.isLoading = false;
        this.dialogRef.close();
        this.getdealers();
        this.spinnerService.stop();
      },
      (exception: any) => {
        this.isLoading = false;
        this.spinnerService.stop();
        this.notificationService.showValidation(exception);
      }
    );
  }
  update() {
    if (this.formGroup.invalid) {
      return;
    }
    let formValue = this.formGroup.value;
    this.isLoading = true;
    this.spinnerService.start();
    this.accountService.updateUser(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("User updated sucessfully.");
        this.isLoading = false;
        this.dialogRef.close();
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
  getRoles() {
    this.accountService.getRoles().subscribe(result => {
      this.roles = result;
    });
  }

}
