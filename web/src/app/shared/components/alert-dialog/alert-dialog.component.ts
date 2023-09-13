import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';

@Component({
  selector: 'app-alert-dialog',
  templateUrl: './alert-dialog.component.html',
  styleUrls: ['./alert-dialog.component.scss']
})
export class AlertDialogComponent implements OnInit {

  title: string;
  message: string;

  constructor(public dialogRef: MatDialogRef<AlertDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: ConfirmationDialogModel) {
      this.title = data.title;
      this.message = data.message;
  }
  ngOnInit(): void {
  }

  onDismiss(): void {
      this.dialogRef.close(false);
  }

}
