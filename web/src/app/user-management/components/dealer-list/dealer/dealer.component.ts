import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BikeFeaturesComponent } from 'src/app/bike-management/components/bike-list/bike/bike-features-list/bike-features/bike-features.component';
import { Bike } from 'src/app/bike-management/models/bike.model';
import { BikeService } from 'src/app/bike-management/services/bike.service';
import { LocationService } from 'src/app/location-management/services/location.service';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { AccountService } from 'src/app/shared/service/account.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { Utilities } from 'src/app/shared/service/utilities';

@Component({
  selector: 'app-dealer',
  templateUrl: './dealer.component.html',
  styleUrls: ['./dealer.component.scss']
})
export class DealerComponent implements OnInit {
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
  constructor(private accountService: AccountService, @Inject(MAT_DIALOG_DATA) public modalData: ModalData, private utilities: Utilities, private spinnerService: SpinnerService, private fb: FormBuilder, public dialogRef: MatDialogRef<BikeFeaturesComponent>, private locationService: LocationService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.checkFormAction(this.modalData);
  }
  checkFormAction(data: ModalData) {
    if (data.action == UserAction.Edit) {
      this.isEdit = true;
      this.formName = "Dealer Local Order"
      this.populateFormData(data.data);
    } 
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      orderId: new FormControl("", [Validators.required]),
      id: new FormControl("", [Validators.required]),
    });
  }
  populateFormData(formData: any) {
    this.formGroup.patchValue(formData);
  }

  get form() {
    return this.formGroup.controls;
  }

  get validationMessage() {
    return ValidationMessage.createDelearForm;
  }
  save() {
    if (this.formGroup.invalid) {
      return;
    }
    let formValue = this.formGroup.value;
    this.isLoading = true;
    let spinerRef = this.spinnerService.start();
    this.accountService.updateDealerOrder(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Local order updated sucessfully.");
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
