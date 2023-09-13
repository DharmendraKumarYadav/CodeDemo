import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ExcelService } from 'src/app/shared/service/excel.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { ShowroomService } from 'src/app/showroom-management/services/showroom.service';

@Component({
  selector: 'app-dealer-available-bikes',
  templateUrl: './dealer-available-bikes.component.html',
  styleUrls: ['./dealer-available-bikes.component.scss']
})
export class DealerAvailableBikesComponent implements OnInit {
  isAdministrator=false;
  formGroup: FormGroup;
  cityList: any[];
  dealerList: any[];
  showRoomList: any[];
  tableDataSource= new MatTableDataSource<any>();
  paginationSizes: number[] = [10, 25, 50,100];
  defaultPageSize = this.paginationSizes[0];
  public sorting = ['bookingDate', 'desc'];
  public startDate = "";
  public finishDate ="";
  displayedColumns = ['variantName', 'exShowRoomPrice', 'insurancePrice', 'rtoPrice', 'price', 'bookingAmount', 'chesisNumber', 'engineNumber','action'];
  @ViewChild(MatPaginator, { static: false }) matPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) matSort: MatSort;
  isLoading: boolean = false;

  isBroker:boolean=false;

  constructor(private authService:AuthenticationService,private showRoomService: ShowroomService,private excelService:ExcelService,private notificationService: NotificationService, private spinnerSerivce: SpinnerService,@Inject(MAT_DIALOG_DATA) public modalData: ModalData, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.getDealerBikes(this.modalData.data.dealerId);
  }
  ngAfterViewInit(){
    this.tableDataSource.paginator = this.matPaginator;
    this.tableDataSource.sort = this.matSort;
  }


  requestBike(userData: any) {
    const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to request bike? ');
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      closeOnNavigation: true,
      data: dialogData
    })

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        let ref = this.spinnerSerivce.start();
        this.showRoomService.requestDealerBike(userData.id).subscribe(result => {
          this.getDealerBikes(this.modalData.data.dealerId);
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
          ("Bike request sent sucessfully.");
        }, (exception: any) => {
          this.isLoading = false;
          this.spinnerSerivce.stop();
          this.notificationService.showValidation(exception);
        });
      } else {

      }
    });
  }

  getDealerBikes(dealerId) {
    this.isLoading = false;
    this.showRoomService.getDealerBikeByUserId(dealerId).subscribe(result => {
      this.tableDataSource = result;
      this.isLoading = true;
    });
  }
  exportToExcel() {
    if(this.tableDataSource.data.length>0){
      const fileName = 'BookingReport';
      const title = 'Bike Booking Report';
      let data = new Array<any>();
      this.tableDataSource.data.forEach(element => {
        let jsonData = [];
        Object.keys(element).forEach(function (key) {
          jsonData.push(element[key]);
        })
        data.push(jsonData)
      });
      const header = ['Booking Id', 'First Name', 'Last Name', 'Phone Number', 'Email', 'Bike Name', 'Booking Amount', 'Booking Date','Payment Status'];
      this.excelService.generateExcel(header, data, title, fileName)
    }
   
  }
 
}
