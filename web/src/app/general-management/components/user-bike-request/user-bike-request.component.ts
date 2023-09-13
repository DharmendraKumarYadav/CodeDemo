import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { Dealer } from 'src/app/showroom-management/models/dealer.model';
import { TableColumn } from 'src/app/shared/components/generic-table/generic-table.component';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { GeneralService } from '../../services/general.service';

@Component({
  selector: 'app-user-bike-request',
  templateUrl: './user-bike-request.component.html',
  styleUrls: ['./user-bike-request.component.scss']
})
export class UserBikeRequestComponent implements OnInit {
  dataSorce: Dealer[];
  tableColumns: TableColumn[];
  isLoading: boolean = false;
  tableTitle: string = "Bike request List";
  addBtnText = "Add ShowRoom";
  modalData = new ModalData();
  isVisibleAddButton=false;

  constructor(private generalService: GeneralService,private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.initializeColumns();
    this.getShowRoomList();
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
        name: 'Mobile',
        dataKey: 'mobileNumber',
        isSortable: false,
        isVisible: true
      },
      {
        name: 'EmailId',
        dataKey: 'email',
        isSortable: true,
        isVisible: true
      },
      {
        name: 'Bike Name',
        dataKey: 'bikeName',
        isSortable: false,
        isVisible: true
      },
      {
        name: 'City',
        dataKey: 'city',
        isSortable: false,
        isVisible: true
      }
    ];
  }

  getShowRoomList() {
    this.isLoading = false;
    this.generalService.getBikeUserRequest(-1,-1).subscribe(result => {
      this.dataSorce = result;
      this.isLoading = true;
    });
  }
}