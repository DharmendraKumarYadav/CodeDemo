import { LocationService } from './../../../services/location.service';
import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { Utilities } from 'src/app/shared/service/utilities';

@Component({
  selector: 'app-city',
  templateUrl: './city.component.html',
  styleUrls: ['./city.component.scss']
})
export class CityComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  formName: string = "Create City"
  isEdit: boolean = false;
  isAdd: boolean = false;
  isView: boolean = false;
  _allowMultiple = true;
  _showCaption = false;
  constructor(@Inject(MAT_DIALOG_DATA) public modalData: ModalData, private utilities: Utilities, private spinnerService: SpinnerService, private fb: FormBuilder, public dialogRef: MatDialogRef<CityComponent>, private locationService: LocationService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.checkFormAction(this.modalData);
  }
  checkFormAction(data: ModalData) {
    if (data.action == UserAction.Edit) {
      this.isEdit = true;
      this.formName = "Edit City"
      this.populateFormData(data.data);
    } else if (data.action == UserAction.Add) {
      this.formName = "Create City"
      this.isAdd = true;
    }
    else if (data.action == UserAction.View) {
      this.formName = "View City"

      this.populateFormData(data.data);
      this.isView = true;
      this.formGroup.disable();
    }
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      name: new FormControl("", [Validators.required])
    });
  }
  populateFormData(formData: any) {
    this.formGroup.patchValue(formData);
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
    this.locationService.createCity(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("City created sucessfully.");
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
    formValue.id = this.modalData.data.id;
    this.isLoading = true;
    let spinerRef = this.spinnerService.start();
    this.locationService.updateCity(formValue, this.modalData.data.id).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("City updated sucessfully.");
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
}
