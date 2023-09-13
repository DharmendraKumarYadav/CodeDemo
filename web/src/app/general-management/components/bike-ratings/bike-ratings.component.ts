import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { ExcelService } from 'src/app/shared/service/excel.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { GeneralService } from '../../services/general.service';

@Component({
  selector: 'app-bike-ratings',
  templateUrl: './bike-ratings.component.html',
  styleUrls: ['./bike-ratings.component.scss']
})
export class BikeRatingsComponent implements OnInit {
  dataSorce= new MatTableDataSource<any>();
  paginationSizes: number[] = [10, 25, 50,100];
  defaultPageSize = this.paginationSizes[0];
  tableColumns:string[] = ['name', 'email', 'bikeName','rating','review','action'];
  @ViewChild(MatPaginator, { static: false }) matPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) matSort: MatSort;

  isLoading: boolean = false;
  constructor(private excelService: ExcelService,private genearlService: GeneralService,private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.getBikeRating();
  }
  ngAfterViewInit(){
    this.dataSorce.paginator = this.matPaginator;
    this.dataSorce.sort = this.matSort;
  }


  getBikeRating() {
    let ref = this.spinnerSerivce.start();
    this.genearlService.getBikeUserRating(-1,-1).subscribe(result => {
      this.dataSorce.data = result;
      this.spinnerSerivce.stop();
    });
  }
  publishRating(item,data){
    let text=item.checked?'Publish':'UnPublish';
    const dialogData = new ConfirmationDialogModel('Confirm', `Are you sure you want to ${text} rating?`);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      closeOnNavigation: true,
      data: dialogData
    })

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        let ref = this.spinnerSerivce.start();
        this.genearlService.updateRating(data.id).subscribe(result => {
          this.getBikeRating();
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
          (`Rating ${text} sucessfully.`);
        });
      } else {
        this.getBikeRating();
      }
    });
  }
  exportToExcel() {
    if(this.dataSorce.data.length>0){
      const fileName = 'BikeRating';
      const title = 'Bike Rating Report';
      let keyArra=['name', 'email', 'bikeName','rating','review'];
      let data = new Array<any>();
      this.dataSorce.data.forEach(element => {
        let jsonData = [];
        Object.keys(element).forEach(function (key) {
          if(keyArra.includes(key)){
            jsonData.push(element[key]);
          }
        })
        data.push(jsonData)
      });
      const header = ['Name', 'Email', 'BikeName','Rating','Review'];
      this.excelService.generateExcel(header, data, title, fileName)
    }
   
  }
}