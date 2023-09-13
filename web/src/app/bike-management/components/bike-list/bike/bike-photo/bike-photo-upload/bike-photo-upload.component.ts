import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BikeService } from 'src/app/bike-management/services/bike.service';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { Utilities } from 'src/app/shared/service/utilities';

@Component({
  selector: 'app-bike-photo-upload',
  templateUrl: './bike-photo-upload.component.html',
  styleUrls: ['./bike-photo-upload.component.scss']
})
export class BikePhotoUploadComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  formName: string = "Add Photo"
  isAdd: boolean = false;
  isView: boolean = false;
  _allowMultiple = true;
  _showCaption = false;
  imageUrl:any
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,private utilities:Utilities, private spinnerService: SpinnerService, private fb: FormBuilder, public dialogRef: MatDialogRef<BikePhotoUploadComponent>, private bikeService: BikeService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.checkFormAction(this.data);
  }
  checkFormAction(data: ModalData) {
     if (data.action == UserAction.Add) {
      this.formName = "Add Photo"
      this.isAdd = true;
      this.formGroup.get("bikeId").setValue(data.data);
    }
    else if (data.action == UserAction.View) {
      this.formName = "View Photo"
      this.isView = true;
      this.imageUrl=data.data;
      this.formGroup.disable();
    }
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      bikeId: new FormControl("", [Validators.required]),
      files: new FormControl("",[Validators.required]),
    });
  }
  populateFormData(formData: any) {
    this.formGroup.patchValue(formData);
  }

  get form() {
    return this.formGroup.controls;
  }

  get validationMessage() {
    return ValidationMessage.createRoleForm;
  }
  save() {
    if (this.formGroup.invalid) {
      return;
    }
    let formValue = this.formGroup.value;
    this.isLoading = true;
    let spinerRef = this.spinnerService.start();
    this.bikeService.saveBikePhoto(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Photo uploaded sucessfully.");
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
  getUploadedFile($event){
    this.formGroup.get("files").setValue($event);
  }
}
