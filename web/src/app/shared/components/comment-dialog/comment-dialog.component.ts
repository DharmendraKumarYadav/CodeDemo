import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmationDialogModel } from '../../model/common/confirm-dialog.model';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
export class DialogResult{
  action:boolean;
  comment:string;
}
@Component({
  selector: 'app-comment-dialog',
  templateUrl: './comment-dialog.component.html',
  styleUrls: ['./comment-dialog.component.scss']
})

export class CommentDialogComponent  implements OnInit {
  formGroup: FormGroup;
  title: string;
  message: string;
   result=new DialogResult();
  constructor(public dialogRef: MatDialogRef<CommentDialogComponent>,private fb: FormBuilder,
      @Inject(MAT_DIALOG_DATA) public data: ConfirmationDialogModel) {
      this.title = data.title;
      this.message = data.message;
  }
  ngOnInit(): void {
    this.initializeForm();
  }
  initializeForm() {
    this.formGroup = this.fb.group({
      comment: new FormControl("", [Validators.required]),

    });
  }

  onConfirm(): void {
      this.result.action=true;
      this.result.comment=this.formGroup.controls["comment"].value;
      this.dialogRef.close(this.result);
  }

  onDismiss(): void {
      this.result.action=false;
      this.result.comment="";
      this.dialogRef.close(this.result);
  }

}
