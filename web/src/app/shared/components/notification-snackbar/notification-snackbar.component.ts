import { Component, Inject, OnInit } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';

@Component({
  selector: 'app-notification-snackbar',
  templateUrl: './notification-snackbar.component.html',
  styleUrls: ['./notification-snackbar.component.scss']
})
export class NotificationSnackbarComponent  {

  constructor(
    public snackBarRef: MatSnackBarRef<NotificationSnackbarComponent>,
    @Inject(MAT_SNACK_BAR_DATA) public data: any
  ) {}


}
