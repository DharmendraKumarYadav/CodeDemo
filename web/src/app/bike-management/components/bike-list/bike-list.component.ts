import { Router } from '@angular/router';
import { BikeService } from './../../services/bike.service';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
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
import { Bike } from '../../models/bike.model';

@Component({
  selector: 'app-bike-list',
  templateUrl: './bike-list.component.html',
  styleUrls: ['./bike-list.component.scss']
})
export class BikeListComponent implements OnInit {
  dataSorce: Bike[];
  tableColumns: TableColumn[];
  isLoading: boolean = false;
  tableTitle: string = "Bike   List";
  addBtnText = "Add Bike";
  modalData = new ModalData();
  constructor(private bikeService: BikeService,private router:Router, private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {

    this.initializeColumns();
    this.loadData();
  }

  sortData(sortParameters: Sort) {
    const keyName = sortParameters.active;
    if (sortParameters.direction === 'asc') {
      this.dataSorce = this.dataSorce.sort((a: Bike, b: Bike) => a[keyName].localeCompare(b[keyName]));
    } else if (sortParameters.direction === 'desc') {
      this.dataSorce = this.dataSorce.sort((a: Bike, b: Bike) => b[keyName].localeCompare(a[keyName]));
    } else {
      // return this.dataSorce = this.getUsers();
    }
  }

  actionClick(rowData: any) {
    if (rowData.action == UserAction.Add) {
      this.router.navigate(['/bike-management/bike'])
    } else if (rowData.action == UserAction.Edit) {
      this.router.navigate(['/bike-management/bike',rowData.row.id])
    } else if (rowData.action == UserAction.Delete) {
      this.delete(rowData.row);
    } 
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
        this.bikeService.deleteBike(rowData.id).subscribe(result => {
          this.loadData();
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Bike deleted sucessfully.");
        });
      } else {

      }
    });
  }

  initializeColumns(): void {
    this.tableColumns = [
      {
        name: 'Id',
        dataKey: 'id',
        isSortable: true,
        isVisible: false
      },
      {
        name: 'Bike Name',
        dataKey: 'name',
        isSortable: true,
        isVisible: true
      },
      {
        name: 'Category Name',
        dataKey: 'category',
        isSortable: false,
        isVisible: true
      },
      {
        name: 'Brand Name',
        dataKey: 'brandName',
        isSortable: false,
        isVisible: true
      },
      {
        name: 'Body Style',
        dataKey: 'bodyStyle',
        isSortable: false,
        isVisible: true
      }


    ];
  }

  loadData() {
    let ref = this.spinnerSerivce.start();
    this.isLoading = false;
    this.bikeService.getBikes(-1,-1)
      .subscribe(results => {
        this.isLoading = true
        this.dataSorce = results;
        this.spinnerSerivce.stop();
      });

  }
}