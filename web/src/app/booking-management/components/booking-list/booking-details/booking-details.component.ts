import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { BikeService } from 'src/app/bike-management/services/bike.service';
import { BookingService } from 'src/app/booking-management/services/booking.service';
import { LocationService } from 'src/app/location-management/services/location.service';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { AccountService } from 'src/app/shared/service/account.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';

@Component({
  selector: 'app-booking-details',
  templateUrl: './booking-details.component.html',
  styleUrls: ['./booking-details.component.scss']
})
export class BookingDetailsComponent implements OnInit {
  formGroup: FormGroup;
  panelOpenState = false;
  formData:any;
  routeState: any;
  constructor(private router:Router,private accountService: AccountService,private bookingService: BookingService, private dialog: MatDialog, private bikeService: BikeService, private locationService: LocationService, private spinnerService: SpinnerService, private fb: FormBuilder, private notificationService: NotificationService) { 
    if (this.router.getCurrentNavigation()?.extras.state) {
      this.routeState = this.router.getCurrentNavigation()?.extras.state;
      if (this.routeState) {
  
    let dataResult= this.routeState.frontEnd
          ? JSON.parse(this.routeState.frontEnd)
          : '';
          this.formData =dataResult.rowData;
      }
    }

  }

  ngOnInit(): void {
  //  this.checkFormAction(this.modalData);
  console.log("Testind detail data",this.formData);
  }

}
