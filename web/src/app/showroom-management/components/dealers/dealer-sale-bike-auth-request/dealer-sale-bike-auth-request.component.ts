import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { ExcelService } from 'src/app/shared/service/excel.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { ShowroomService } from 'src/app/showroom-management/services/showroom.service';

@Component({
  selector: 'app-dealer-sale-bike-auth-request',
  templateUrl: './dealer-sale-bike-auth-request.component.html',
  styleUrls: ['./dealer-sale-bike-auth-request.component.scss']
})
export class DealerSaleBikeAuthRequestComponent implements OnInit {
  isAdministrator = false;
  formGroup: FormGroup;
  cityList: any[];
  dealerList: any[];
  showRoomList: any[];
  tableDataSource = new MatTableDataSource<any>();
  paginationSizes: number[] = [10, 25, 50, 100];
  defaultPageSize = this.paginationSizes[0];
  public sorting = ['bookingDate', 'desc'];
  public startDate = "";
  public finishDate = "";
  displayedColumns = ['requestorName', 'variantName', 'exShowRoomPrice', 'insurancePrice', 'rtoPrice', 'price', 'bookingAmount', 'chesisNumber', 'engineNumber', 'action'];
  @ViewChild(MatPaginator, { static: false }) matPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) matSort: MatSort;
  isLoading: boolean = false;


  constructor(private showRoomService: ShowroomService, private excelService: ExcelService, private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.getBrokerBikeRequests();
  }
  ngAfterViewInit() {
    this.tableDataSource.paginator = this.matPaginator;
    this.tableDataSource.sort = this.matSort;
  }

  requestBike(userData: any, action) {
    let request = {
      "id": userData.id,
      "transferStatus": action,

    }
    const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to proceed? ');
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      closeOnNavigation: true,
      data: dialogData
    })

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        let ref = this.spinnerSerivce.start();
        this.showRoomService.authorizeBrokerRequest(request).subscribe(result => {
          this.getBrokerBikeRequests();
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Bike action done sucessfully.");
        });
      }
    });
  }


  getBrokerBikeRequests() {
    this.isLoading = false;
    this.showRoomService.getBrokerBikeRequest(-1, -1).subscribe(result => {
      this.tableDataSource = result;
      this.isLoading = true;
    });
  }
  exportToExcel() {
    if (this.tableDataSource.data.length > 0) {
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
      const header = ['Booking Id', 'First Name', 'Last Name', 'Phone Number', 'Email', 'Bike Name', 'Booking Amount', 'Booking Date', 'Payment Status'];
      this.excelService.generateExcel(header, data, title, fileName)
    }

  }

}
