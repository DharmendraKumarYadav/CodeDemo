import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
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
import { BrokerBikeSalesAddUpdateComponent } from '../broker-bike-sales-add-update/broker-bike-sales-add-update.component';

@Component({
  selector: 'app-broker-bike-sales',
  templateUrl: './broker-bike-sales.component.html',
  styleUrls: ['./broker-bike-sales.component.scss']
})
export class BrokerBikeSalesComponent implements OnInit {
  paginationSizes: number[] = [10, 25, 50, 100];
  defaultPageSize = this.paginationSizes[0];
  public tableDataSource = new MatTableDataSource([]);
  displayedColumns: string[] = ['variantName', 'exShowRoomPrice', 'insurancePrice', 'rtoPrice','price','bookingAmount','chesisNumber','engineNumber', 'action'];
  @ViewChild(MatPaginator, { static: false }) matPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) matSort: MatSort;
  isLoading: boolean = false;
  modalData = new ModalData();
  tableTitle: string = "Broker Bike List";
  constructor(private excelService:ExcelService,private showRoomService: ShowroomService,public authService: AuthenticationService,private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.getSaleBikes();
  }
  ngAfterViewInit(): void {
    this.tableDataSource.paginator = this.matPaginator;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.tableDataSource.filter = filterValue.trim().toLowerCase();
  }

  edit(data: any) {
    this.modalData.action = "EDIT";
    this.modalData.data = data;
    const dialogRef = this.dialog.open(BrokerBikeSalesAddUpdateComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getSaleBikes();
    });
  }
  return(data: any){
    const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to return bike? ');
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      closeOnNavigation: true,
      data: dialogData
    })

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        let ref = this.spinnerSerivce.start();
        this.showRoomService.returnDealerBike(data.id).subscribe(result => {
          this.getSaleBikes();
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Bike returned sucessfully.");
        });
      }
    });
  }


  getSaleBikes() {
    this.isLoading = false;
    let userId=this.authService.currentUser.id;
    this.showRoomService.getBrokerSaleBike(userId,-1,-1).subscribe(result => {
      this.tableDataSource = new MatTableDataSource<any>(result);
      this.tableDataSource.paginator = this.matPaginator;
      this.tableDataSource.sort = this.matSort;
      this.isLoading = true;
    });
  }
  exportToExcel() {
    if (this.tableDataSource.data.length > 0) {
      const fileName = this.tableTitle;
      const title = this.tableTitle + 'Report';
      const header = [];
      const keyArra: string[] = ['name', 'mobileNumber', 'emailId', 'status'];
      let data = new Array<any>();
      this.tableDataSource.data.forEach(element => {
        let jsonData = [];
        Object.keys(element).forEach(function (key) {
          if (keyArra.includes(key)) {
            jsonData.push(element[key]);
            // if (!header.includes(pascalCase(titleCase(key)))) {
            //   header.push(pascalCase(titleCase(key)));
            // }

          }
        })
        data.push(jsonData)
      });
      this.excelService.generateExcel(header, data, title, fileName)
    }
  }
 
}