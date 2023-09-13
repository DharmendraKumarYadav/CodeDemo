import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { BikeFeaturesComponent } from 'src/app/bike-management/components/bike-list/bike/bike-features-list/bike-features/bike-features.component';
import { Bike } from 'src/app/bike-management/models/bike.model';
import { BikeService } from 'src/app/bike-management/services/bike.service';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { TableColumn } from 'src/app/shared/components/generic-table/generic-table.component';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { User } from 'src/app/shared/model/auth/user.model';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { AccountService } from 'src/app/shared/service/account.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { UserComponent } from '../user/user.component';
import { DealerComponent } from './dealer/dealer.component';

@Component({
  selector: 'app-dealer-list',
  templateUrl: './dealer-list.component.html',
  styleUrls: ['./dealer-list.component.scss']
})
export class DealerListComponent  implements OnInit {
  dataSorce: User[];
  tableColumns: TableColumn[];
  isLoading: boolean = false;
  tableTitle: string = "Dealer List";
  addBtnText = "Add User";
  modalData = new ModalData();

  constructor(private accountService: AccountService, private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.initializeColumns();
    this.getUsers();
  }

  sortData(sortParameters: Sort) {
    const keyName = sortParameters.active;
    if (sortParameters.direction === 'asc') {
      this.dataSorce = this.dataSorce.sort((a: User, b: User) => a[keyName].localeCompare(b[keyName]));
    } else if (sortParameters.direction === 'desc') {
      this.dataSorce = this.dataSorce.sort((a: User, b: User) => b[keyName].localeCompare(a[keyName]));
    }
  }

  actionClick(rowData: any) {
   if (rowData.action == UserAction.Edit) {
      this.editUser(rowData.row);
    }
  }


  editUser(userData: any) {
    this.modalData.action = "EDIT";
    this.modalData.data = userData;
    const dialogRef = this.dialog.open(DealerComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getUsers();
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
        name: 'Name',
        dataKey: 'name',
        isSortable: true,
        isVisible: true
      },
      {
        name: 'Local Order',
        dataKey: 'localOrder',
        isSortable: false,
        isVisible: true
      }
    ];
  }

  getUsers() {
    this.spinnerSerivce.start();
    this.accountService.getDealers().subscribe(result => {
      this.dataSorce = result;
      this.spinnerSerivce.stop();
    }, (error: any) => {
      this.spinnerSerivce.stop();
    });
  }
}