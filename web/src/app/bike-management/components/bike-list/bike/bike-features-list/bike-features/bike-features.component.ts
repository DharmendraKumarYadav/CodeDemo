import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Bike } from 'src/app/bike-management/models/bike.model';
import { BikeService } from 'src/app/bike-management/services/bike.service';
import { LocationService } from 'src/app/location-management/services/location.service';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { Utilities } from 'src/app/shared/service/utilities';

@Component({
  selector: 'app-bike-features',
  templateUrl: './bike-features.component.html',
  styleUrls: ['./bike-features.component.scss']
})
export class BikeFeaturesComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  bikes: Bike[]
  types = [{
    id: 1,
    name: 'Popular'
  }, {
    id: 2,
    name: 'New Launch'
  }, {
    id: 3,
    name: 'Upcoming'
  }]
  formName: string = "Create Feature Type"
  isEdit: boolean = false;
  isAdd: boolean = false;
  isView: boolean = false;
  _allowMultiple = true;
  _showCaption = false;
  constructor(private bikeService: BikeService, @Inject(MAT_DIALOG_DATA) public modalData: ModalData, private utilities: Utilities, private spinnerService: SpinnerService, private fb: FormBuilder, public dialogRef: MatDialogRef<BikeFeaturesComponent>, private locationService: LocationService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadData();
    this.checkFormAction(this.modalData);
  }
  checkFormAction(data: ModalData) {
    if (data.action == UserAction.Edit) {
      this.isEdit = true;
      this.formName = "Edit Feature Type"
      this.populateFormData(data.data);
    } else if (data.action == UserAction.Add) {
      this.formName = "Create Feature Type"
      this.isAdd = true;
    }
    else if (data.action == UserAction.View) {
      this.formName = "View Feature Type"

      this.populateFormData(data.data);
      this.isView = true;
      this.formGroup.disable();
    }
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      typeId: new FormControl("", [Validators.required]),
      bikeId: new FormControl("", [Validators.required]),
    });
  }
  populateFormData(formData: any) {
    this.formGroup.patchValue(formData);
  }

  get form() {
    return this.formGroup.controls;
  }

  get validationMessage() {
    return ValidationMessage.bikeFeatureForm;
  }
  save() {
    if (this.formGroup.invalid) {
      return;
    }
    let formValue = this.formGroup.value;
    this.isLoading = true;
    let spinerRef = this.spinnerService.start();
    this.bikeService.saveFeatureBikeType(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Feature created sucessfully.");
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
    this.bikeService.updateFeatureBikeType(formValue, this.modalData.data.id).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Feature updated sucessfully.");
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
    this.bikeService.getBikes(-1, -1)
      .subscribe(results => {
        this.isLoading = true
        this.bikes = results;
      });

  }
}
