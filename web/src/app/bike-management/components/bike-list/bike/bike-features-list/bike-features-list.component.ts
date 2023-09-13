import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { Bike } from 'src/app/bike-management/models/bike.model';
import { BikeService } from 'src/app/bike-management/services/bike.service';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { TableColumn } from 'src/app/shared/components/generic-table/generic-table.component';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { BikeFeaturesComponent } from './bike-features/bike-features.component';

@Component({
  selector: 'app-bike-features-list',
  templateUrl: './bike-features-list.component.html',
  styleUrls: ['./bike-features-list.component.scss']
})
export class BikeFeaturesListComponent implements OnInit {
  dataSorce: Bike[];
  tableColumns: TableColumn[];
  isLoading: boolean = false;
  tableTitle: string = "Features Bike List";
  addBtnText = "Assign Bike Category";
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
      this.create();
    } else if (rowData.action == UserAction.Edit) {
      this.edit(rowData.row);
    } else if (rowData.action == UserAction.Delete) {
      this.delete(rowData.row);
    } else if (rowData.action == UserAction.View) {
      this.view(rowData.row);
    }
  }


  create() {
    this.modalData.action = "ADD";
    this.modalData.data = null;
    const dialogRef = this.dialog.open(BikeFeaturesComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.loadData();
    });
  }
  edit(rowData: any) {
    this.modalData.action = "EDIT";
    this.modalData.data = rowData;
    const dialogRef = this.dialog.open(BikeFeaturesComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.loadData();
    });
  }
  view(rowData: any) {
    this.modalData.action = "VIEW";
    this.modalData.data = rowData;
    const dialogRef = this.dialog.open(BikeFeaturesComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
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
        this.bikeService.deleteFeatureBikeType(rowData.id).subscribe(result => {
          this.loadData();
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Featured deleted sucessfully.");
            this.isLoading = false;
        });
      } else {
        this.isLoading = false;
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
        name: 'Type',
        dataKey: 'type',
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
    this.bikeService.getFeatureBikeType(-1,-1)
      .subscribe(results => {
        this.isLoading = false
        this.dataSorce = results;
        
        this.spinnerSerivce.stop();
      });

  }
}