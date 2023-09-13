import { map } from 'rxjs/operators';
import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { Bike } from 'src/app/bike-management/models/bike.model';
import { BikeService } from 'src/app/bike-management/services/bike.service';
import { LocationService } from 'src/app/location-management/services/location.service';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { TableColumn } from 'src/app/shared/components/generic-table/generic-table.component';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { Utilities } from 'src/app/shared/service/utilities';
import { BikeFeaturesComponent } from '../bike-features-list/bike-features/bike-features.component';

@Component({
  selector: 'app-related-bike',
  templateUrl: './related-bike.component.html',
  styleUrls: ['./related-bike.component.scss']
})
export class RelatedBikeComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  bikes:Bike[]
  types=[{
    id:0,
    name:'Popular'
  },{
    id:1,
    name:'New Launch'
  },{
    id:2,
    name:'Upcoming'
  },{
    id:3,
    name:'Scooters'
  },{
    id:4,
    name:'Sports'
  },
  {
    id:5,
    name:'Cruiser'
  },{
    id:6,
    name:'Electric Bikes'
  }]
  formName: string = "Create Feature Type"
  isEdit: boolean = false;
  isAdd: boolean = false;
  isView: boolean = false;
  _allowMultiple = true;
  _showCaption = false;
  constructor(private bikeService: BikeService,@Inject(MAT_DIALOG_DATA) public modalData: ModalData,private utilities:Utilities, private spinnerService: SpinnerService, private fb: FormBuilder, public dialogRef: MatDialogRef<BikeFeaturesComponent>, private locationService: LocationService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadData();
    this.checkFormAction(this.modalData);
  }
  checkFormAction(data: ModalData) {
    if (data.action == UserAction.Edit) {
      this.isEdit = true;
      this.formName = "Edit Similar Bike"
      this.populateFormData(data.data);
    } else if (data.action == UserAction.Add) {
      this.formName = "Create Similar Bike"
      this.isAdd = true;
    }
    else if (data.action == UserAction.View) {
      this.formName = "View Similar Bike"

      this.populateFormData(data.data);
      this.isView = true;
      this.formGroup.disable();
    }
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      similarBikeId: new FormControl("",[Validators.required]),
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
    formValue.bikeId=this.modalData.data;
    this.isLoading = true;
    let spinerRef = this.spinnerService.start();
    this.bikeService.saveBikeRelated(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Similar bike created sucessfully.");
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

  }
  loadData() {
    this.isLoading = false;
    this.bikeService.getBikes(-1,-1)
      .subscribe(results => {
        this.bikes=new Array<Bike>();
        this.isLoading = true
        results.forEach(element => {
          if(element.id!=this.modalData.data)
          this.bikes.push(element)
        });
      
      });

  }
}
