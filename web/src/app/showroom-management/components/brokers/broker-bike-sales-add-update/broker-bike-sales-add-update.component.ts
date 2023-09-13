import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Bike } from 'src/app/bike-management/models/bike.model';
import { BikeService } from 'src/app/bike-management/services/bike.service';
import { Colour } from 'src/app/bike-spec-management/models/colour.model';
import { BikeSpecificationService } from 'src/app/bike-spec-management/services/bike-specification.service';
import { BikeVariantsComponent } from 'src/app/booking-management/components/booking-list/bike-variants/bike-variants.component';
import { BikeVariantPrice } from 'src/app/booking-management/models/bike-variant-price.model';
import { BookingService } from 'src/app/booking-management/services/booking.service';
import { ShowRoom, City, BikeAvailbility } from 'src/app/location-management/models/city.model';
import { LocationService } from 'src/app/location-management/services/location.service';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { User } from 'src/app/shared/model/auth/user.model';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { AccountService } from 'src/app/shared/service/account.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { NumericValidator } from 'src/app/shared/validators/common-validator';
import { ShowroomService } from 'src/app/showroom-management/services/showroom.service';

@Component({
  selector: 'app-broker-bike-sales-add-update',
  templateUrl: './broker-bike-sales-add-update.component.html',
  styleUrls: ['./broker-bike-sales-add-update.component.scss']
})
export class BrokerBikeSalesAddUpdateComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  formName: string = "New Bike"

  showRooms = new Array<ShowRoom>();
  bikes: Bike[];
  users: User[];
  city: City[];
  colours: Colour[];
  bikeAvailbility = new Array<BikeAvailbility>()
  bikeVariantPrice = new Array<BikeVariantPrice>();

  selectedBikePrice = new BikeVariantPrice();


  isEdit: boolean = false;
  isAdd: boolean = false;
  isView: boolean = false;
  constructor(private router: Router,private showRoomService: ShowroomService, private bikeSpecService: BikeSpecificationService, public dialogRef: MatDialogRef<BrokerBikeSalesAddUpdateComponent>, @Inject(MAT_DIALOG_DATA) public modalData: ModalData, private accountService: AccountService, private bookingService: BookingService, private dialog: MatDialog, private bikeService: BikeService, private locationService: LocationService, private spinnerService: SpinnerService, private fb: FormBuilder, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.getShowRoomList();
    this.getColourList();
    this.populateBikeAvailability();
    this.checkFormAction(this.modalData);
  }
  checkFormAction(data: ModalData) {
    if (data.action == UserAction.Edit) {
      this.formName = "Edit ShoBikeswRoom"
      this.populateFormData(data.data);
    } 
    
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      showRoomId: new FormControl("", [Validators.required]),
      bikeVariantsId: new FormControl("", [Validators.required]),
      bookingAmount: new FormControl("", [Validators.required, NumericValidator]),
      rTOPrice: new FormControl("", [Validators.required, NumericValidator]),
      insurancePrice: new FormControl("", [Validators.required, NumericValidator]),
      exShowRoomPrice: new FormControl("", [Validators.required, NumericValidator]),
      bikeColurId: new FormControl("", [Validators.required, NumericValidator]),
      availbility: new FormControl("", [Validators.required]),
      isMinPrice: new FormControl(null),
      noOfDay: new FormControl("", [Validators.required, NumericValidator]),
      chesisNumber: new FormControl("", [Validators.required]),
      engineNumber: new FormControl("", [Validators.required])
    });
  }
  populateFormData(formData: any) {
    this.formGroup.patchValue(formData);
    this.formGroup.get('rTOPrice').setValue(formData.rtoPrice);
    this.loadShowRoomData(formData.showRoomId, formData.bikeVariantsId)
  }

  get form() {
    return this.formGroup.controls;
  }

  get validationMessage() {
    return ValidationMessage.saleBikeForm;
  }

  update() {
    if (this.formGroup.invalid) {
      return;
    }
    let formValue = this.formGroup.getRawValue();
    this.isLoading = true;
    formValue.id = this.modalData.data.id;

    let spinerRef = this.spinnerService.start();
    this.showRoomService.updateBrokerBike(formValue, formValue.id).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Bike updated sucessfully.");
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
  onSelectBikeVariant(varaintData) {
    let item = this.bikeVariantPrice.find(m => m.id == varaintData);
    this.selectedBikePrice=item;
    // this.formGroup.get('bookingAmount').setValue(item.bookingAmount);
    // this.formGroup.get('rTOPrice').setValue(item.rtoAmount);
    // this.formGroup.get('insurancePrice').setValue(item.insuranceAmount);
    // this.formGroup.get('exShowRoomPrice').setValue(item.exShowRoomAmount);
    this.formGroup.get('isMinPrice').setValue(item.isMinPrice);
    // this.formGroup.get('bikeVariantsId').setValue(item.id);
    this.updatePriceValidators();
    this.formGroup.get('bikeVariantsId').disable();
    this.formGroup.get('bikeColurId').disable();
    this.formGroup.get('chesisNumber').disable();
    this.formGroup.get('engineNumber').disable();
  }
  updatePriceValidators(){
      if (this.selectedBikePrice.isMinPrice) {
        this.formGroup.controls['bookingAmount'].setValidators([Validators.min(+this.selectedBikePrice.bookingAmount)]);
        this.formGroup.controls['rTOPrice'].setValidators([Validators.min(+this.selectedBikePrice.rtoAmount)]);
        this.formGroup.controls['insurancePrice'].setValidators([Validators.min(+this.selectedBikePrice.insuranceAmount)]);
        this.formGroup.controls['exShowRoomPrice'].setValidators([Validators.min(+this.selectedBikePrice.exShowRoomAmount)]);
      } else {
        this.formGroup.controls['bookingAmount'].clearValidators();
        this.formGroup.controls['rTOPrice'].clearValidators();
        this.formGroup.controls['insurancePrice'].clearValidators();
        this.formGroup.controls['exShowRoomPrice'].clearValidators();
      }
      this.formGroup.controls['rTOPrice'].updateValueAndValidity();
      this.formGroup.controls['exShowRoomPrice'].updateValueAndValidity();
      this.formGroup.controls['insurancePrice'].updateValueAndValidity();
      this.formGroup.controls['bookingAmount'].updateValueAndValidity();
  
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
  getShowRoomList() {
    this.isLoading = false;
    this.showRoomService.getShowRooms(-1, -1).subscribe(result => {
      result.map(m => {
        this.showRooms.push({ id: m.id, name: m.name });
      });
      this.isLoading = true;
    });
  }
  getColourList() {
    this.bikeSpecService.getColour(-1, -1).subscribe(m => {
      this.colours = m;
    });
  }
  onSelectShowRoom($event) {

  }
  calculateValues() {

  }
  populateBikeAvailability() {
    this.bikeAvailbility.push({
      id: "1", name: 'Redy to Sell',

    });
    this.bikeAvailbility.push({
      id: "2", name: 'Wating(No. Of Days)'

    });

  }
  onSelectAvailbility($event) {
    if ($event.value == "1") {
      this.formGroup.get('noOfDay').setValue(0);
      this.formGroup.get('noOfDay').disable();
    }else{
      this.formGroup.get('noOfDay').enable();
    }
  }
  loadShowRoomData(showRoomId, variantId) {
    let spinerRef = this.spinnerService.start();
    this.bikeService.getBikeVariantPriceByCity(showRoomId)
      .subscribe((results: BikeVariantPrice[]) => {
        this.isLoading = true;
        this.bikeVariantPrice = results;
        let item = this.bikeVariantPrice.find(m => m.id == variantId);
        this.formGroup.get('bikeVariantsId').setValue(item.id);
        this.onSelectBikeVariant(item.id);
        this.spinnerService.stop();
      }, (exception: any) => {
        this.isLoading = false;
        this.spinnerService.stop();
        this.notificationService.showValidation(exception);
      });
  }
}
