import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-view-comment-dialog',
  templateUrl: './view-comment-dialog.component.html',
  styleUrls: ['./view-comment-dialog.component.scss']
})
export class ViewCommentDialogComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {}

  ngOnInit(): void {
  }
  onDismiss(){
    
  }
}
