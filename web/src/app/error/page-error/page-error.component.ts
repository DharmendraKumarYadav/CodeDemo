import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApplicationMessage } from 'src/app/shared/message/application-message';
import { AuthenticationService } from 'src/app/shared/service/auth.service';

@Component({
  selector: 'app-page-error',
  templateUrl: './page-error.component.html',
  styleUrls: ['./page-error.component.scss']
})
export class PageErrorComponent implements OnInit {
  errorObject: any;
  errorImg = require("../../../assets/logo/logo.png");
  constructor(private route: ActivatedRoute, private authService: AuthenticationService) {
    this.route.queryParams.subscribe(params => {
      let errorCode = params?.errorcode;
      this.errorMessage(errorCode);
    }
    );
  }

  ngOnInit(): void {

  }
  errorMessage(code: string) {
    let item = ApplicationMessage.ERROR_MESSAGE.find((item) => item.code.toLocaleLowerCase() == code.toLocaleLowerCase());
    if (item == undefined) {
      
    } else {
      this.errorObject = item == undefined ? ApplicationMessage.COMMON_MESSAGE : item;
    }
  }
}
