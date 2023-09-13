import { BodyStyleComponent } from './body-style/body-style.component';
import { BodyStyle } from './../../models/body-style.model';
import { Attribute, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { TableColumn } from 'src/app/shared/components/generic-table/generic-table.component';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { BikeSpecificationService } from '../../services/bike-specification.service';
import { AttributeComponent } from '../attribute/attribute.component';
import { Utilities } from 'src/app/shared/service/utilities';

@Component({
  selector: 'app-body-style-list',
  templateUrl: './body-style-list.component.html',
  styleUrls: ['./body-style-list.component.scss']
})
export class BodyStyleListComponent implements OnInit {
  dataSorce: BodyStyle[];
  tableColumns: TableColumn[];
  isLoading: boolean = false;
  tableTitle: string = "BodyStyle List";
  addBtnText = "Add BodyStyle";
  modalData = new ModalData();
  constructor(public utilities: Utilities,private bikeSpecService: BikeSpecificationService, private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {

    this.initializeColumns();
    this.getAttributes();
  }

  sortData(sortParameters: Sort) {
    const keyName = sortParameters.active;
    if (sortParameters.direction === 'asc') {
      this.dataSorce = this.dataSorce.sort((a: BodyStyle, b: BodyStyle) => a[keyName].localeCompare(b[keyName]));
    } else if (sortParameters.direction === 'desc') {
      this.dataSorce = this.dataSorce.sort((a: BodyStyle, b: BodyStyle) => b[keyName].localeCompare(a[keyName]));
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
    const dialogRef = this.dialog.open(BodyStyleComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getAttributes();
    });
  }
  edit(rowData: any) {
    this.modalData.action = "EDIT";
    this.modalData.data = rowData;
    const dialogRef = this.dialog.open(BodyStyleComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getAttributes();
    });
  }
  view(rowData: any) {
    this.modalData.action = "VIEW";
    this.modalData.data = rowData;
    const dialogRef = this.dialog.open(BodyStyleComponent, {
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
        this.bikeSpecService.deleteBodyStyle(rowData.id).subscribe(result => {
          this.getAttributes();
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("BodyStyle deleted sucessfully.");
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
        name: 'Image',
        dataKey: 'imageUrl',
        isSortable: false,
        isVisible: true
      },
      {
        name: 'Description',
        dataKey: 'description',
        isSortable: false,
        isVisible: true
      }


    ];
  }

  getAttributes() {
    this.isLoading = false;
    this.bikeSpecService.getBodyStyle(-1,-1)
      .subscribe(results => {
        this.isLoading = true
        results.map(m => {
          m.imageUrl = m.images!=null?this.utilities.getImagePathFromBase64(m.images.base64Content):"";
          return m;
        });
        console.log(results)
        this.dataSorce = results;
      });

  }
}