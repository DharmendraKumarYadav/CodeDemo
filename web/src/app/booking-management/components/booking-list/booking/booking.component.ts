import { ShowroomService } from './../../../../showroom-management/services/showroom.service';
import { Router } from '@angular/router';
import { BookingService } from './../../../services/booking.service';
import { BikeVariantsComponent } from './../bike-variants/bike-variants.component';
import { User } from 'src/app/shared/model/auth/user.model';
import { BikeService } from 'src/app/bike-management/services/bike.service';
import { AccountService } from 'src/app/shared/service/account.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { LocationService } from 'src/app/location-management/services/location.service';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { Bike } from 'src/app/bike-management/models/bike.model';
import { City } from 'src/app/location-management/models/city.model';
import { BikeVariantPrice } from 'src/app/booking-management/models/bike-variant-price.model';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.scss']
})
export class BookingComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  formName: string = "New Booking"


  bikes: Bike[];
  users: User[];
  city: City[];
  bikeVariantPrice = new Array<BikeVariantPrice>();

  modalData = new ModalData();

  isEdit: boolean = false;
  isAdd: boolean = false;
  isView: boolean = false;
  constructor(private router:Router,private accountService: AccountService,private bookingService: BookingService, private dialog: MatDialog, private bikeService: BikeService, private locationService: LocationService, private spinnerService: SpinnerService, private fb: FormBuilder, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.getArea();
  }


  initializeForm() {
    this.formGroup = this.fb.group({
      bikeId: new FormControl("", [Validators.required]),
      userId: new FormControl("", [Validators.required]),
      name: new FormControl("", [Validators.required]),
      phoneNumber: new FormControl("", [Validators.required]),
      email: new FormControl("", [Validators.required]),
      amount: new FormControl("", [Validators.required]),
      address: new FormControl("", [Validators.required]),
      cityId: new FormControl("", [Validators.required]),
      bikeVariantPriceId: new FormControl("", [Validators.required]),

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
    this.bookingService.createBooking(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Booking created sucessfully.");
        this.isLoading = false;
        this.spinnerService.stop();
        this.router.navigate(['/booking-management/booking'])
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

    // let spinerRef = this.spinnerService.start();
    // this.dealerService.updateDealer(formValue, formValue.id).subscribe(
    //   (response: any) => {
    //     this.notificationService.showSuccess
    //       ("Booking updated sucessfully.");
    //     this.isLoading = false;
    //     this.spinnerService.stop();
    //   },
    //   (exception: any) => {
    //     this.isLoading = false;
    //     this.spinnerService.stop();
    //     this.notificationService.showValidation(exception);
    //   }
    // );
  }
  resteForm() {

  }
  getArea() {
    this.locationService.getCity(-1, -1).subscribe(result => {
      this.city = result;
    });
    this.accountService.getUsers().subscribe(result => {
      this.users = result;
    });
    this.bikeService.getBikes(-1, -1)
      .subscribe(results => {
        this.isLoading = true
        this.bikes = results;
      });
  }
  onSelectBike($event) {
    let formValue = this.formGroup.value;
    this.getBikeVariantDetails(formValue.bikeId, formValue.cityId);
  }
  getBikeVariantDetails(bikeId, cityId) {
    let spinerRef = this.spinnerService.start();
    this.bikeService.getBikeVariantPrice(bikeId, cityId)
      .subscribe(results => {
        this.isLoading = true
        this.showBikeVariants(results);
        this.spinnerService.stop();
      }, (exception: any) => {
        this.isLoading = false;
        this.spinnerService.stop();
        this.notificationService.showValidation(exception);
      });
  }
  showBikeVariants(variantData: BikeVariantPrice[]) {
    const dialogRef = this.dialog.open(BikeVariantsComponent, {
      data: variantData
    });
    dialogRef.afterClosed().subscribe((result: BikeVariantPrice) => {
      this.formGroup.get('bikeVariantPriceId').setValue(result.id);
      this.formGroup.get('amount').setValue(result.bookingAmount);   
    });
  }
  onSelectUser($event) {
    this.formGroup.get('name').setValue($event.value.fullName);
    this.formGroup.get('email').setValue($event.value.email);
    this.formGroup.get('phoneNumber').setValue($event.value.phoneNumber);
    this.formGroup.get('userId').setValue($event.value.id);  
 
  }
}
