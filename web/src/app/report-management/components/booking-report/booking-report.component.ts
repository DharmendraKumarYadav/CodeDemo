import { ExcelService } from '../../../shared/service/excel.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { forkJoin } from 'rxjs';
import { ReportService } from '../../services/report.service';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { AuthenticationService } from 'src/app/shared/service/auth.service';

@Component({
  selector: 'app-booking-report',
  templateUrl: './booking-report.component.html',
  styleUrls: ['./booking-report.component.scss']
})
export class BookingReportComponent implements OnInit {
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
  displayedColumns = ['bookingId', 'firstName', 'lastName', 'phoneNumber', 'email', 'bikeName', 'bookingAmount','bookingDate', 'paymentStatus'];
  @ViewChild(MatPaginator, { static: false }) matPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) matSort: MatSort;

  constructor(private authService: AuthenticationService,private spinnerService: SpinnerService,private excelService: ExcelService,private fb: FormBuilder,  private reportService: ReportService) {}
 
  ngOnInit(): void {
    this.initializeForm();
    this.getInitialData();
    this.isAdministrator=this.authService.isAdminstrator;
  }
  ngAfterViewInit(){
    this.tableDataSource.paginator = this.matPaginator;
    this.tableDataSource.sort = this.matSort;
  }
  initializeForm() {
    this.formGroup = this.fb.group({
      cityId: new FormControl(0),
      dealerId: new FormControl(""),
      showRoomId: new FormControl(0),
      startDate: new FormControl(""),
      endDate: new FormControl(""),
    });
  }
  getInitialData() {
    let city = this.reportService.getCity();
    let dealers = this.reportService.getDealers();
    let showRooms = this.reportService.getShowRoom();
    forkJoin([city, dealers,showRooms]).subscribe((result:any[]) => {
      this.cityList = result[0];
      this.dealerList = result[1];
      this.showRoomList=result[2];
    });
    this.onSearch();
  }
  changeDate(number, date) {
    if (number === 0) {
      this.startDate = date
    }
    if (number === 1) {
      this.finishDate = date
    }
  };
  onSearch() {
    let formvalue=this.formGroup.value;
    let filter = {
      "startDate": this.startDate,
      "endDate": this.finishDate,
      "cityId":formvalue.cityId,
      "dealerId":formvalue.dealerId,
      "showRoomId": formvalue.showRoomId,
    }
    let ref=this.spinnerService.start();
    this.reportService.getBokingDetail(filter).subscribe(m => {
      this.tableDataSource.data = m;
      this.spinnerService.stop();
    })
  }
  onFilterClear() {
    this.initializeForm();
    let filter = {
      "startDate": "",
      "endDate": "",
      "cityId": 0,
      "dealerId":"",
      "showRoomId": 0
    }
    this.reportService.getBokingDetail(filter).subscribe(m => {
      this.tableDataSource = m;
    })
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
