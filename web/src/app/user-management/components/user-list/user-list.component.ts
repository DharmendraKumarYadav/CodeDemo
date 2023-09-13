import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { ModalData } from './../../../shared/model/common/modal-data.model';
import { UserComponent } from './../user/user.component';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { AccountService } from 'src/app/shared/service/account.service';
import { TableColumn } from 'src/app/shared/components/generic-table/generic-table.component';
import { User } from 'src/app/shared/model/auth/user.model';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { UserAction } from 'src/app/shared/enums/table-action.enum';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  dataSorce: User[];
  tableColumns: TableColumn[];
  isLoading: boolean = false;
  tableTitle: string = "User List";
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
    if (rowData.action == UserAction.Add) {
      this.createUser();
    } else if (rowData.action == UserAction.Edit) {
      this.editUser(rowData.row);
    } else if (rowData.action == UserAction.Delete) {
      this.deleteUser(rowData.row);
    } else if (rowData.action == UserAction.View) {
      this.viewUser(rowData.row);
    }
  }

  createUser() {
    this.modalData.action = "ADD";
    this.modalData.data = null;
    const dialogRef = this.dialog.open(UserComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getUsers();
    });
  }
  editUser(userData: any) {
    this.modalData.action = "EDIT";
    this.modalData.data = userData;
    const dialogRef = this.dialog.open(UserComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getUsers();
    });
  }
  viewUser(userData: any) {
    this.modalData.action = "VIEW";
    this.modalData.data = userData;
    const dialogRef = this.dialog.open(UserComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getUsers();
    });
  }
  deleteUser(userData: any) {
    const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to delete user? ');
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      closeOnNavigation: true,
      data: dialogData
    })

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        let ref = this.spinnerSerivce.start();
        this.accountService.deleteUser(userData).subscribe(result => {
          this.getUsers();
          this.spinnerSerivce.stop();
          this.notificationService.showSuccess
            ("User deleted sucessfully.");
        }, (error => {
          this.notificationService.showSuccess
            (error.error);
          this.spinnerSerivce.stop();
        }));
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
        name: 'Name',
        dataKey: 'fullName',
        isSortable: true,
        isVisible: true
      },
      {
        name: 'User Name',
        dataKey: 'userName',
        isSortable: false,
        isVisible: true
      },
      {
        name: 'Email',
        dataKey: 'email',
        isSortable: true,
        isVisible: true
      },
      {
        name: 'Phone Number',
        dataKey: 'phoneNumber',
        isSortable: false,
        isVisible: true
      },
      {
        name: 'Role',
        dataKey: 'role',
        isSortable: false,
        isVisible: true
      },
      {
        name: 'Status',
        dataKey: 'isEnabled',
        isSortable: false,
        isVisible: true

      },
      {
        name: 'Locked',
        dataKey: 'isLockedOut',
        isSortable: false,
        isVisible: false

      },
    ];
  }

  getUsers() {
    this.spinnerSerivce.start();
    this.accountService.getUsers().subscribe(result => {
      this.dataSorce = result;
      this.spinnerSerivce.stop();
    }, (error: any) => {
      this.spinnerSerivce.stop();
    });
  }
}