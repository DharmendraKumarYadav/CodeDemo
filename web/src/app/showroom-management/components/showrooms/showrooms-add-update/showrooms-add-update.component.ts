import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Area } from 'src/app/location-management/models/area.model';
import { LocationService } from 'src/app/location-management/services/location.service';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { ValidateMobileNumber } from 'src/app/shared/validators/common-validator';
import { ShowroomService } from 'src/app/showroom-management/services/showroom.service';

@Component({
  selector: 'app-showrooms-add-update',
  templateUrl: './showrooms-add-update.component.html',
  styleUrls: ['./showrooms-add-update.component.scss']
})
export class ShowroomsAddUpdateComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  formName: string = "Create Show Room"
  area: Area[];
  isEdit: boolean = false;
  isAdd: boolean = false;
  isView: boolean = false;
  constructor(@Inject(MAT_DIALOG_DATA) public modalData: ModalData, private locationService:LocationService,private spinnerService: SpinnerService, private fb: FormBuilder, public dialogRef: MatDialogRef<ShowroomsAddUpdateComponent>, private showRoomService: ShowroomService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.getArea();
    this.checkFormAction(this.modalData);
  }
  checkFormAction(data: ModalData) {
    if (data.action == UserAction.Edit) {
      this.isEdit = true;
      this.formName="Edit ShowRoom"
      this.populateFormData(data.data);
    } else if (data.action == UserAction.Add) {
      this.formName="Create ShowRoom"
      this.isAdd = true;
    }
    else if (data.action == UserAction.View) {
      this.formName="View ShowRoom"
      this.populateFormData(data.data);
      this.isView = true;
      this.formGroup.disable();
    }
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      name: new FormControl("", [Validators.required]),
      mobileNumber: new FormControl("", [Validators.required,ValidateMobileNumber]),
      emailId: new FormControl("", [Validators.required,Validators.email]),
      phoneNumber: new FormControl("", [Validators.required,ValidateMobileNumber]),
      addressLine2: new FormControl("", [Validators.required]),
      addressLine1: new FormControl("", [Validators.required]),
      urlLink: new FormControl(""),
      areaId: new FormControl("", [Validators.required]),

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
    this.showRoomService.createShowRoom(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Showroom created sucessfully.");
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
    this.isLoading = true;
    formValue.id = this.modalData.data.id;
    this.spinnerService.start();
    this.showRoomService.updateShowRoom(formValue, formValue.id).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Showroom updated sucessfully.");
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
  getArea() {
    this.locationService.getArea(-1,-1).subscribe(result => {
      this.area = result;
    });
  }

}
