import { Utilities } from './../../../shared/service/utilities';
import { BrandComponent } from './brand/brand.component';
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
import { Brand } from '../../models/brand.model';
import { BikeSpecificationService } from '../../services/bike-specification.service';
import { AttributeComponent } from '../attribute/attribute.component';

@Component({
  selector: 'app-brand-list',
  templateUrl: './brand-list.component.html',
  styleUrls: ['./brand-list.component.scss']
})
export class BrandListComponent implements OnInit {
  dataSorce: Brand[];
  tableColumns: TableColumn[];
  isLoading: boolean = false;
  tableTitle: string = "Brand List";
  addBtnText = "Add Brand";
  modalData = new ModalData();
  constructor(public utilities: Utilities, private bikeSpecService: BikeSpecificationService, private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {

    this.initializeColumns();
    this.getBrands();
  }

  sortData(sortParameters: Sort) {
    const keyName = sortParameters.active;
    if (sortParameters.direction === 'asc') {
      this.dataSorce = this.dataSorce.sort((a: Brand, b: Brand) => a[keyName].localeCompare(b[keyName]));
    } else if (sortParameters.direction === 'desc') {
      this.dataSorce = this.dataSorce.sort((a: Brand, b: Brand) => b[keyName].localeCompare(a[keyName]));
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
    const dialogRef = this.dialog.open(BrandComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getBrands();
    });
  }
  edit(rowData: any) {
    this.modalData.action = "EDIT";
    this.modalData.data = rowData;
    const dialogRef = this.dialog.open(BrandComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getBrands();
    });
  }
  view(rowData: any) {
    this.modalData.action = "VIEW";
    this.modalData.data = rowData;
    const dialogRef = this.dialog.open(AttributeComponent, {
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
        this.bikeSpecService.deleteBrand(rowData.id).subscribe(result => {
          this.getBrands();
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Brand deleted sucessfully.");
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
        name: 'Brand Logo',
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

  getBrands() {
    this.isLoading = false;
    this.bikeSpecService.getBrand(-1, -1)
      .subscribe(results => {
        this.isLoading = true
       results.map(m => {
          m.imageUrl = this.utilities.getImagePathFromBase64(m.images.base64Content);
          return m;
        });
        this.dataSorce = results;
      });

  }
}