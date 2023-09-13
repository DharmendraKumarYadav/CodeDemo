import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ExcelService } from 'src/app/shared/service/excel.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { ShowroomService } from '../../services/showroom.service';


@Component({
  selector: 'app-booked-bikes',
  templateUrl: './booked-bikes.component.html',
  styleUrls: ['./booked-bikes.component.scss']
})
export class BookedBikesComponent implements OnInit {
  paginationSizes: number[] = [10, 25, 50, 100];
  defaultPageSize = this.paginationSizes[0];
  public tableDataSource = new MatTableDataSource([]);
  displayedColumns: string[] = ['variantName', 'exShowRoomPrice', 'insurancePrice', 'rtoPrice','price','bookingAmount','chesisNumber','engineNumber'];
  @ViewChild(MatPaginator, { static: false }) matPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) matSort: MatSort;
  isLoading: boolean = false;
  modalData = new ModalData();
  tableTitle: string = "Booked Bikes";
  constructor(private excelService:ExcelService,private showRoomService: ShowroomService,public authService: AuthenticationService,private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.getBookedBike();
  }
  ngAfterViewInit(): void {
    this.tableDataSource.paginator = this.matPaginator;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.tableDataSource.filter = filterValue.trim().toLowerCase();
  }


  getBookedBike() {
    this.isLoading = false;
    this.showRoomService.getBookedBike(-1,-1).subscribe(result => {
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