import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import{ValidationDialogModel} from 'src/app/shared/model/common/Validation-dialog.model'


@Component({
  selector: 'app-validation-dialog',
  templateUrl: './validation-dialog.component.html',
  styleUrls: ['./validation-dialog.component.scss']
})
export class ValidationDialogComponent implements OnInit {

  title: string;
  message: string;

  constructor(public dialogRef: MatDialogRef<ValidationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ValidationDialogModel) {
    data.error.forEach(element => {
      this.message=element;
    });
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
