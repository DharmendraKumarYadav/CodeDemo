import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { TableColumn } from 'src/app/shared/components/generic-table/generic-table.component';
import { User } from 'src/app/shared/model/auth/user.model';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { AccountService } from 'src/app/shared/service/account.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { ShowroomService } from '../../services/showroom.service';
import { DealerSaleBikeListViewComponent } from './dealer-sale-bike-list-view/dealer-sale-bike-list-view.component';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { DealerAvailableBikesComponent } from './dealer-available-bikes/dealer-available-bikes.component';


@Component({
  selector: 'app-dealers',
  templateUrl: './dealers.component.html',
  styleUrls: ['./dealers.component.scss']
})
export class DealersComponent implements OnInit {
  dataSorce: User[];
  tableColumns: TableColumn[];
  isLoading: boolean = false;
  tableTitle: string = "Dealer List";
  modalData = new ModalData();

  constructor(private authService:AuthenticationService,private showRoomService:ShowroomService,private accountService: AccountService, private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.initializeColumns();
    this.getDealers();
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
    this.viewUser(rowData.row);
    
  }
  viewUser(userData: any) {
    if(this.authService.isBroker){
      this.modalData.action = "VIEW";
      this.modalData.data = userData;
      const dialogRef = this.dialog.open(  DealerAvailableBikesComponent, {
        data: this.modalData
      });
      dialogRef.afterClosed().subscribe(result => {
        // this.getShowRoomList();
      });
    }else{
      this.modalData.action = "VIEW";
      this.modalData.data = userData;
      const dialogRef = this.dialog.open( DealerSaleBikeListViewComponent, {
        data: this.modalData
      });
      dialogRef.afterClosed().subscribe(result => {
        // this.getShowRoomList();
      });
    }

  }



  initializeColumns(): void {
    this.tableColumns = [
      {
        name: 'Id',
        dataKey: 'dealerId',
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
        name: 'Email',
        dataKey: 'email',
        isSortable: true,
        isVisible: true
      },
      {
        name: 'Mobile Number',
        dataKey: 'mobile',
        isSortable: false,
        isVisible: true
      }
    ];
  }

  getDealers() {
    this.spinnerSerivce.start();
    this.showRoomService.getDealers().subscribe(result => {
      this.dataSorce = result;
      this.spinnerSerivce.stop();
    }, (error: any) => {
      this.spinnerSerivce.stop();
    });
  }

}
