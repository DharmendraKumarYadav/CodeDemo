import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent implements OnInit {
  
  title: string;
  message: string;

  constructor(public dialogRef: MatDialogRef<ConfirmDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: ConfirmationDialogModel) {
      this.title = data.title;
      this.message = data.message;
  }
  ngOnInit(): void {
  }

  onConfirm(): void {
      this.dialogRef.close(true);
  }

  onDismiss(): void {
      this.dialogRef.close(false);
  }

}
