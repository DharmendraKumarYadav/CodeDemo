import { DisplacementComponent } from './displacement/displacement.component';
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
import { Displacement } from '../../models/displacement.model';
import { BikeSpecificationService } from '../../services/bike-specification.service';

@Component({
  selector: 'app-displacement-list',
  templateUrl: './displacement-list.component.html',
  styleUrls: ['./displacement-list.component.scss']
})
export class DisplacementListComponent implements OnInit {
  dataSorce: Displacement[];
  tableColumns: TableColumn[];
  isLoading: boolean = false;
  tableTitle: string = "Displacement List";
  addBtnText = "Add Displacement";
  modalData = new ModalData();
  constructor(private bikeSpecService: BikeSpecificationService, private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {

    this.initializeColumns();
    this.loadData();
  }

  sortData(sortParameters: Sort) {
    const keyName = sortParameters.active;
    if (sortParameters.direction === 'asc') {
      this.dataSorce = this.dataSorce.sort((a: Displacement, b: Displacement) => a[keyName].localeCompare(b[keyName]));
    } else if (sortParameters.direction === 'desc') {
      this.dataSorce = this.dataSorce.sort((a: Displacement, b: Displacement) => b[keyName].localeCompare(a[keyName]));
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
    const dialogRef = this.dialog.open(DisplacementComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.loadData();
    });
  }
  edit(rowData: any) {
    this.modalData.action = "EDIT";
    this.modalData.data = rowData;
    const dialogRef = this.dialog.open(DisplacementComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.loadData();
    });
  }
  view(rowData: any) {
    this.modalData.action = "VIEW";
    this.modalData.data = rowData;
    const dialogRef = this.dialog.open(DisplacementComponent, {
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
        this.bikeSpecService.deleteDisplacement(rowData.id).subscribe(result => {
          this.loadData();
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Displacement deleted sucessfully.");
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
        dataKey: 'name',
        isSortable: true,
        isVisible: true
      },
      {
        name: 'Value',
        dataKey: 'value',
        isSortable: false,
        isVisible: true
      },
      {
        name: 'Operator',
        dataKey: 'operator',
        isSortable: false,
        isVisible: true
      }



    ];
  }

  loadData() {
    this.isLoading = false;
    this.bikeSpecService.getDisplacement(-1,-1)
      .subscribe(results => {
        this.isLoading = true
        this.dataSorce = results;
      });

  }
}