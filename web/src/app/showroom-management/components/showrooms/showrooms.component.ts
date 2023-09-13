import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CommentDialogComponent, DialogResult } from 'src/app/shared/components/comment-dialog/comment-dialog.component';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ViewCommentDialogComponent } from 'src/app/shared/components/view-comment-dialog/view-comment-dialog.component';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ExcelService } from 'src/app/shared/service/excel.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { ShowroomService } from '../../services/showroom.service';
import { ShowroomsAddUpdateComponent } from './showrooms-add-update/showrooms-add-update.component';

@Component({
  selector: 'app-showrooms',
  templateUrl: './showrooms.component.html',
  styleUrls: ['./showrooms.component.scss']
})
export class ShowroomsComponent implements OnInit {
  paginationSizes: number[] = [10, 25, 50, 100];
  defaultPageSize = this.paginationSizes[0];
  public tableDataSource = new MatTableDataSource([]);
  displayedColumns: string[] = ['name', 'mobileNumber', 'emailId', 'status', 'action'];
  @ViewChild(MatPaginator, { static: false }) matPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) matSort: MatSort;
  isLoading: boolean = false;
  modalData = new ModalData();
  tableTitle: string = "Show Room List";
  constructor(public authService: AuthenticationService, private showRoomService: ShowroomService, public excelService: ExcelService, private notificationService: NotificationService, private spinnerSerivce: SpinnerService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.getShowRoomList();
  }

  ngAfterViewInit(): void {
    this.tableDataSource.paginator = this.matPaginator;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.tableDataSource.filter = filterValue.trim().toLowerCase();
  }
  //Intial Load Data
  getShowRoomList() {
    this.isLoading = false;
  
    this.showRoomService.getAllShowRooms(-1, -1).subscribe(result => {
      this.tableDataSource = new MatTableDataSource<any>(result);
      this.tableDataSource.paginator = this.matPaginator;
      this.tableDataSource.sort = this.matSort;
      this.isLoading = false;
    });
  }


  create() {
    this.modalData.action = "ADD";
    this.modalData.data = null;
    const dialogRef = this.dialog.open(ShowroomsAddUpdateComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getShowRoomList();
    });
  }
  edit(userData: any) {
    this.modalData.action = "EDIT";
    this.modalData.data = userData;
    const dialogRef = this.dialog.open(ShowroomsAddUpdateComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getShowRoomList();
    });
  }
  view(userData: any) {
    this.modalData.action = "VIEW";
    this.modalData.data = userData;
    const dialogRef = this.dialog.open(ShowroomsAddUpdateComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getShowRoomList();
    });
  }
  delete(userData: any) {
    const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to delete user? ');
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      closeOnNavigation: true,
      data: dialogData
    })

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        let ref = this.spinnerSerivce.start();
        this.showRoomService.deleteShowRoom(userData.id).subscribe(result => {
          this.getShowRoomList();
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Show Room deleted sucessfully.");
        });
      }
    });
  }
  authorizeRequest(data, action) {
    let request = {
      "id": data.id,
      "status": action,
      "comments": ""
    }
    const dialogData = new ConfirmationDialogModel('Your Comment', 'Are you sure you want to proceed? ');
    const dialogRef = this.dialog.open(CommentDialogComponent, {
      maxWidth: '400px',
      closeOnNavigation: true,
      data: dialogData
    })

    dialogRef.afterClosed().subscribe((dialogResult: DialogResult) => {
      if (dialogResult.action) {
        request.comments = dialogResult.comment;
        let ref = this.spinnerSerivce.start();
        this.showRoomService.authorizeShowRoomRequest(request).subscribe(result => {
          this.getShowRoomList();
          this.spinnerSerivce.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Action done sucessfully.");
        });
      }
    });
  }
  viewComment(row){
    this.dialog.open(ViewCommentDialogComponent, {
      data:row.comments,

    });
  
  }

  exportToExcel() {
    if (this.tableDataSource.data.length > 0) {
      const fileName = this.tableTitle;
      const title = this.tableTitle + 'Report';
      const header = [];
      const keyArra: string[] = ['name', 'mobileNumber', 'emailId', 'status'];
      let data = new Array<any>();
      this.tableDataSource.data.forEach(element => {
        let jsonData = [];
        Object.keys(element).forEach(function (key) {
          if (keyArra.includes(key)) {
            jsonData.push(element[key]);
            // if (!header.includes(pascalCase(titleCase(key)))) {
            //   header.push(pascalCase(titleCase(key)));
            // }

          }
        })
        data.push(jsonData)
      });
      this.excelService.generateExcel(header, data, title, fileName)
    }
  }


}