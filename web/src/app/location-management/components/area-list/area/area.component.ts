import { City } from 'src/app/shared/model/common/city.model';
import { LocationService } from './../../../services/location.service';
import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BrandComponent } from 'src/app/bike-spec-management/components/brand-list/brand/brand.component';
import { BikeSpecificationService } from 'src/app/bike-spec-management/services/bike-specification.service';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { Utilities } from 'src/app/shared/service/utilities';
import { LengthValidator, NumericValidator } from 'src/app/shared/validators/common-validator';

@Component({
  selector: 'app-area',
  templateUrl: './area.component.html',
  styleUrls: ['./area.component.scss']
})
export class AreaComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  city: City[]
  formName: string = "Create Area"
  isEdit: boolean = false;
  isAdd: boolean = false;
  isView: boolean = false;
  _allowMultiple = true;
  _showCaption = false;
  constructor(@Inject(MAT_DIALOG_DATA) public modalData: ModalData, private utilities: Utilities, private spinnerService: SpinnerService, private fb: FormBuilder, public dialogRef: MatDialogRef<AreaComponent>, private locationService: LocationService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadData();
    this.checkFormAction(this.modalData);
  }
  checkFormAction(data: ModalData) {
    if (data.action == UserAction.Edit) {
      this.isEdit = true;
      this.formName = "Edit Area"
      this.populateFormData(data.data);
    } else if (data.action == UserAction.Add) {
      this.formName = "Create Area"
      this.isAdd = true;
    }
    else if (data.action == UserAction.View) {
      this.formName = "View Area"

      this.populateFormData(data.data);
      this.isView = true;
      this.formGroup.disable();
    }
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      name: new FormControl("", [Validators.required]),
      pinCode: new FormControl("", [Validators.required,NumericValidator,LengthValidator(6)]),
      cityId: new FormControl("", [Validators.required]),
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
    this.locationService.createArea(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Area created sucessfully.");
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
    this.spinnerService.start();
    this.locationService.updateArea(formValue, this.modalData.data.id).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Area updated sucessfully.");
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
  loadData() {
    this.isLoading = false;
    this.locationService.getCity(-1, -1)
      .subscribe(results => {
        this.isLoading = true
        this.city = results;
      });

  }
}
