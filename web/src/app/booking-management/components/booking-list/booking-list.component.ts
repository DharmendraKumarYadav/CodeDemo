import { Router } from '@angular/router';
import { BookingComponent } from './booking/booking.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort, Sort } from '@angular/material/sort';
import { CategoryComponent } from 'src/app/bike-spec-management/components/category/category.component';
import { Category } from 'src/app/bike-spec-management/models/category.model';
import { BikeSpecificationService } from 'src/app/bike-spec-management/services/bike-specification.service';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { TableColumn } from 'src/app/shared/components/generic-table/generic-table.component';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { BookingService } from '../../services/booking.service';
import { MatTableDataSource } from '@angular/material/table';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { MatPaginator } from '@angular/material/paginator';
import { ShowroomService } from 'src/app/showroom-management/services/showroom.service';
import { ExcelService } from 'src/app/shared/service/excel.service';
import { CommentDialogComponent, DialogResult } from 'src/app/shared/components/comment-dialog/comment-dialog.component';
import { BookingDetailsComponent } from './booking-details/booking-details.component';

@Component({
  selector: 'app-booking-list',
  templateUrl: './booking-list.component.html',
  styleUrls: ['./booking-list.component.scss']
})
export class BookingListComponent implements OnInit {

  paginationSizes: number[] = [10, 25, 50, 100];
  defaultPageSize = this.paginationSizes[0];
  public tableDataSource = new MatTableDataSource([]);
  displayedColumns: string[] = ['bookingNumber', 'chesisNumber', 'engineNumber', 'phoneNumber', 'bookingDate', 'amount', 'paymentStatus','bookingStatus','action'];
  @ViewChild(MatPaginator, { static: false }) matPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) matSort: MatSort;
  isLoading: boolean = false;
  modalData = new ModalData();
  tableTitle: string = "Booking List";
  // constructor(public authService: AuthenticationService, private showRoomService: ShowroomService, public excelService: ExcelService, private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  // }


  dataSorce: any[];
  tableColumns: TableColumn[];

  isAddVisible=false;
  constructor(private showRoomService: ShowroomService, public excelService: ExcelService, private notificationService: NotificationService,public authService: AuthenticationService,private router: Router,private bookingService:BookingService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.loadData();
  }
  ngAfterViewInit(): void {
    this.tableDataSource.paginator = this.matPaginator;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.tableDataSource.filter = filterValue.trim().toLowerCase();
  }

  sortData(sortParameters: Sort) {
    const keyName = sortParameters.active;
    if (sortParameters.direction === 'asc') {
      this.dataSorce = this.dataSorce.sort((a: Category, b: Category) => a[keyName].localeCompare(b[keyName]));
    } else if (sortParameters.direction === 'desc') {
      this.dataSorce = this.dataSorce.sort((a: Category, b: Category) => b[keyName].localeCompare(a[keyName]));
    } else {
      // return this.dataSorce = this.getUsers();
    }
  }

  actionClick(rowData: any) {
    if (rowData.action == UserAction.Add) {
      this.router.navigate(['/booking-management/booking-details'])
     } 
    else if (rowData.action == UserAction.Delete) {
      this.delete(rowData.row);
    } 
  }

  create() {
    this.modalData.action = "ADD";
    this.modalData.data = null;
    const dialogRef = this.dialog.open(BookingComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.loadData();
    });
  }
  edit(rowData: any) {
    this.modalData.action = "EDIT";
    this.modalData.data = rowData;
    const dialogRef = this.dialog.open(BookingComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.loadData();
    });
  }
  view(rowData: any) {
    // this.modalData.action = "VIEW";
    // this.modalData.data = rowData;
    // const dialogRef = this.dialog.open(BookingDetailsComponent, {
    //   data: this.modalData,
    //   width:"100%"
    // });
    // dialogRef.afterClosed().subscribe(result => {
    // });
    this.router.navigate(['/booking-management/booking-details'], {
      state: {
        frontEnd: JSON.stringify({ rowData })
      },
 });
  }
  delete(rowData: any) {
    const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to delete? ');
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      closeOnNavigation: true,
      data: dialogData
    })

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        let ref = this.spinnerSerivce.start();
        this.bookingService.deleteBooking(rowData.id).subscribe(result => {
          this.loadData();
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Booking deleted sucessfully.");
        });
      } else {

      }
    });
  }

  loadData() {
    this.isLoading = false;
    this.bookingService.getBookings(-1,-1)
      .subscribe(results => {
        this.tableDataSource = new MatTableDataSource<any>(results);
      this.tableDataSource.paginator = this.matPaginator;
      this.tableDataSource.sort = this.matSort;
      this.isLoading = false;
      });

  }
  authorizeRequest(data, action) {
    let request = {
      "id": data.id,
      "status": action,
      "bookingNumber":data.bookingNumber
    }
    const dialogData = new ConfirmationDialogModel('Your Comment', 'Are you sure you want to proceed? ');
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      closeOnNavigation: true,
      data: dialogData
    })

    dialogRef.afterClosed().subscribe((dialogResult: DialogResult) => {
      if (dialogResult) {
        let ref = this.spinnerSerivce.start();
        this.bookingService.authorizeBooking(request).subscribe(result => {
          this.loadData();
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Action done sucessfully.");
        });
      }
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