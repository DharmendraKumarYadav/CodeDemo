import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { ExcelService } from 'src/app/shared/service/excel.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { ShowroomService } from 'src/app/showroom-management/services/showroom.service';
import { DealerSaleBikeAddUpdateComponent } from '../../dealers/dealer-sale-bike-add-update/dealer-sale-bike-add-update.component';

@Component({
  selector: 'app-broker-bike-sales-list-view',
  templateUrl: './broker-bike-sales-list-view.component.html',
  styleUrls: ['./broker-bike-sales-list-view.component.scss']
})
export class BrokerBikeSalesListViewComponent implements OnInit {
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
  displayedColumns = ['variantName', 'exShowRoomPrice', 'insurancePrice', 'rtoPrice', 'price', 'bookingAmount', 'chesisNumber', 'engineNumber'];
  @ViewChild(MatPaginator, { static: false }) matPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) matSort: MatSort;
  isLoading: boolean = false;

  isEdit: boolean = false;
  isAdd: boolean = false;
  isView: boolean = false;

  constructor( public dialogRef: MatDialogRef<DealerSaleBikeAddUpdateComponent>, @Inject(MAT_DIALOG_DATA) public modalData: ModalData,private showRoomService: ShowroomService, private excelService: ExcelService, private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.checkFormAction(this.modalData);
  }
  ngAfterViewInit() {
    this.tableDataSource.paginator = this.matPaginator;
    this.tableDataSource.sort = this.matSort;
  }
  checkFormAction(data: ModalData) {
    this.getBrokerBikeRequests(data.data.brokerId);
  }



  getBrokerBikeRequests(userId:string) {
    this.isLoading = false;
    this.showRoomService.getBrokerSaleBike(userId,-1, -1).subscribe(result => {
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
