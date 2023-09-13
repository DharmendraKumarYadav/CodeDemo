import { Utilities } from './../../../shared/service/utilities';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { AccountService } from 'src/app/shared/service/account.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { UserComponent } from 'src/app/user-management/components/user/user.component';
import { Specification } from '../../models/specification.model';
import { BikeSpecificationService } from '../../services/bike-specification.service';

@Component({
  selector: 'app-specification',
  templateUrl: './specification.component.html',
  styleUrls: ['./specification.component.scss']
})
export class SpecificationComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  formName: string = "Create Specification"
  roles: Specification[];
  isEdit: boolean = false;
  isAdd: boolean = false;
  isView: boolean = false;
  _allowMultiple = true;
  _showCaption = false;
  imageUrl:any;
  constructor(@Inject(MAT_DIALOG_DATA) public modalData: ModalData,private utilities:Utilities, private spinnerService: SpinnerService, private fb: FormBuilder, public dialogRef: MatDialogRef<UserComponent>, private bikeSpecService: BikeSpecificationService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.checkFormAction(this.modalData);
  }
  checkFormAction(data: ModalData) {
    if (data.action == UserAction.Edit) {
      this.isEdit = true;
      this.formName = "Edit Specification"
      this.populateFormData(data.data);
    } else if (data.action == UserAction.Add) {
      this.formName = "Create Specification"
      this.isAdd = true;
    }
    else if (data.action == UserAction.View) {
      this.formName = "View Specification"

      this.populateFormData(data.data);
      this.isView = true;
      this.formGroup.disable();
    }
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      name: new FormControl("", [Validators.required]),
      description: new FormControl("", [Validators.required]),
      files: new FormControl([],[Validators.required])
    });
  }
  populateFormData(formData: any) {
    this.formGroup.patchValue(formData);
    this.imageUrl =formData.images!=null? this.utilities.getImagePathFromBase64(formData.images.base64Content):"";
    this.formGroup.get('files').clearValidators();
    this.formGroup.updateValueAndValidity();
  }

  get form() {
    return this.formGroup.controls;
  }

  get validationMessage() {
    return ValidationMessage.catalogForm;
  }
  save() {
    if (this.formGroup.invalid) {
      return;
    }
    let formValue = this.formGroup.value;
    this.isLoading = true;
    this.spinnerService.start();
    this.bikeSpecService.createSpecification(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Specification created sucessfully.");
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
  update() {
    if (this.formGroup.invalid) {
      return;
    }
    let formValue = this.formGroup.value;
    formValue.id=this.modalData.data.id;
    this.isLoading = true;
    let spinerRef = this.spinnerService.start();
    this.bikeSpecService.updateSpecification(formValue,this.modalData.data.id).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Specification updated sucessfully.");
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
    this.initializeForm();
  }
  getUploadedFile($event){
    console.log("eventfileData");
    this.formGroup.get("files").setValue($event);
  }
}
