import { error } from 'protractor';
import { Observable, of, Subject } from 'rxjs';
import { Injectable, NgZone } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { AlertDialogComponent } from 'src/app/shared/components/alert-dialog/alert-dialog.component';
import { AuthenticationService } from './auth.service';
import { ValidationDialogModel } from 'src/app/shared/model/common/Validation-dialog.model';
import { ValidationDialogComponent } from 'src/app/shared/components/validation-dialog/validation-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  result: Observable<any>;;

  constructor(private snackBar: MatSnackBar, public dialog: MatDialog, private zone: NgZone, private authService: AuthenticationService) {

  }
  showSuccess(message: string): void {
    const dialogData = new ConfirmationDialogModel('Message', message);
    this.dialog.open(AlertDialogComponent, {
      panelClass: 'trend-dialog',
      maxWidth: '400px',
      data: dialogData
    })
  }

  showError(message: string): void {
    const dialogData = new ConfirmationDialogModel('Message', message);
    this.dialog.open(AlertDialogComponent, {
      panelClass: 'trend-dialog',
      maxWidth: '400px',
      data: dialogData
    })
  }

  //message=  exception.message;

  showValidation(exception: any): void {
    let message = new Array<string>();
    if (exception.status == 500 || exception.status == 400 || exception.status == 401) {
      if(exception.error?.ServerValidation){
        message = exception.error.ServerValidation;
      }else{
        message.push(exception.error);
      }
    
    }
    else {
      message.push("we are facing some technical issue please try after some time");
    }
    const dialogData = new ValidationDialogModel('Message', message,);
    this.dialog.open(ValidationDialogComponent, {
      panelClass: 'trend-dialog',
      maxWidth: '400px',
      data: dialogData
    })
  }
  public openSnackBar(exception: any) {
    let message = this.getMessage(exception);
    const dialogData = new ConfirmationDialogModel('Message', message);
    this.dialog.open(AlertDialogComponent, {
      maxWidth: '400px',
      data: dialogData
    })

  }
  getMessage(exception: any) {
    if (exception?.error?.error == "invalid_grant") {
    if(exception?.error?.error_description=="user_inactive"){
      return "User is disabled.Please contact adminstrator.";
    }
    else if(exception?.error?.error_description =="phone_unverified")
    {
      return "Phone number is not verified.";
    }
    else if(exception?.error?.error_description =="unauthorize_access")
    {
      return "Unauthroize access.";
    }
    else if(exception?.error?.error_description=="user_notfound")
    {
      return "User not found.";
    }
    else{
      return "Invalid Username or Password";
    }
    } else if (exception?.error?.ServerValidation) {
      return exception.error.ServerValidation;
    }else{
      return "We are facing technical issues, Please try later.";

    }
  }

}
