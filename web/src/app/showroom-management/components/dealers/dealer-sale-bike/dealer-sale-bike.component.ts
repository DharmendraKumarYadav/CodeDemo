import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { TableColumn } from 'src/app/shared/components/generic-table/generic-table.component';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { Dealer } from 'src/app/showroom-management/models/dealer.model';
import { ShowroomService } from 'src/app/showroom-management/services/showroom.service';
import { DealerSaleBikeAddUpdateComponent } from '../dealer-sale-bike-add-update/dealer-sale-bike-add-update.component';

@Component({
  selector: 'app-dealer-sale-bike',
  templateUrl: './dealer-sale-bike.component.html',
  styleUrls: ['./dealer-sale-bike.component.scss']
})
export class DealerSaleBikeComponent implements OnInit {
  dataSorce: Dealer[];
  tableColumns: TableColumn[];
  isLoading: boolean = false;
  tableTitle: string = "Dealer Bikes";
  addBtnText = "Add Bikes";
  modalData = new ModalData();

  constructor(private showRoomService: ShowroomService,private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.initializeColumns();
    this.getDealerBikes();
  }

  sortData(sortParameters: Sort) {
    const keyName = sortParameters.active;
    if (sortParameters.direction === 'asc') {
      this.dataSorce = this.dataSorce.sort((a: Dealer, b: Dealer) => a[keyName].localeCompare(b[keyName]));
    } else if (sortParameters.direction === 'desc') {
      this.dataSorce = this.dataSorce.sort((a: Dealer, b: Dealer) => b[keyName].localeCompare(a[keyName]));
    } else {
      // return this.dataSorce = this.getUsers();
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
    const dialogRef = this.dialog.open(DealerSaleBikeAddUpdateComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getDealerBikes();
    });
  }
  editUser(userData: any) {
    this.modalData.action = "EDIT";
    this.modalData.data = userData;
    const dialogRef = this.dialog.open(DealerSaleBikeAddUpdateComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getDealerBikes();
    });
  }
  viewUser(userData: any) {
    this.modalData.action = "VIEW";
    this.modalData.data = userData;
    const dialogRef = this.dialog.open(DealerSaleBikeAddUpdateComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getDealerBikes();
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
        this.showRoomService.deleteDealerBike(userData.id).subscribe(result => {
          this.getDealerBikes();
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
        name: 'Name',
        dataKey: 'variantName',
        isSortable: true,
        isVisible: true
      },
      {
        name: 'ShowRoom',
        dataKey: 'exShowRoomPrice',
        isSortable: true,
        isVisible: true
      },
      {
        name: 'Insurance',
        dataKey: 'insurancePrice',
        isSortable: true,
        isVisible: true
      },
      {
        name: 'RTO',
        dataKey: 'rtoPrice',
        isSortable: true,
        isVisible: true
      },
      {
        name: 'Total',
        dataKey: 'price',
        isSortable: true,
        isVisible: true
      },
      
      {
        name: 'Booking Amount',
        dataKey: 'bookingAmount',
        isSortable: false,
        isVisible: true
      },
      {
        name: 'Chesis Number',
        dataKey: 'chesisNumber',
        isSortable: true,
        isVisible: true
      },
      {
        name: 'Engine Number',
        dataKey: 'engineNumber',
        isSortable: false,
        isVisible: true
      }
    ];
  }

  getDealerBikes() {
    this.isLoading = false;
    this.showRoomService.getDealerBike(-1,-1).subscribe(result => {
      this.dataSorce = result;
      this.isLoading = true;
    });
  }
 
}