import { LoginModel } from '../../../shared/model/auth/login.model';
import { Component, ElementRef, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { SharedService } from 'src/app/shared/service/shared.service';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { MatDialog } from '@angular/material/dialog';
import { HttpUrlEncodingCodec } from '@angular/common/http';
import { SpinnerService } from 'src/app/shared/service/spinner.service';

@Component({
  selector: 'app-login',
  encapsulation: ViewEncapsulation.None,
  templateUrl: './login.component.html',
  styleUrls: [
  ]

})
export class LoginComponent implements OnInit {
  hide = true;
  model = new LoginModel();
  loginForm: FormGroup;
  loading: boolean;
  data: any;
  isChecked: boolean = false;
  constructor(private router: Router,private titleService: Title, 
    private notificationService: NotificationService, public dialog: MatDialog, private authenticationService: AuthenticationService,private spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    this.titleService.setTitle('Login');
    this.initializeForm();
    this.spinnerService.stop();

  }
  private initializeForm() {
    this.loginForm = new FormGroup({
      userid: new FormControl("admin", [Validators.required]),
      password: new FormControl('Password@123', Validators.required)
    });

  }

  login() {
    if (this.loginForm.invalid) {
      return;
    }
    let formValue = this.loginForm.value;
    this.model.userid = formValue.userid;
    this.model.password = formValue.password;
    this.spinnerService.start();
    this.authenticationService
      .loginWithPassword( this.model.userid, this.model.password,true)
      .subscribe(
        () => {
          this.spinnerService.stop();
          this.router.navigate(['/dashboard'])
        },
        (exception: any) => {
          this.notificationService.openSnackBar(exception);
          this.spinnerService.stop();
        }
      );

  }
  get form() { return this.loginForm.controls; }

  get validationMessage() {
    return ValidationMessage.loginForm;
  }
}
