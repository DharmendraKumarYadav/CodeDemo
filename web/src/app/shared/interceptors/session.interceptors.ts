import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpResponse,
} from "@angular/common/http";
import { Observable } from "rxjs";
import { SpinnerService } from "../service/spinner.service";
import { tap } from "rxjs/operators";
import { error } from "protractor";
import { Injectable } from "@angular/core";
import { AuthenticationService } from "../service/auth.service";

@Injectable()
export class SessionInterceptor implements HttpInterceptor {
  constructor(
    private spinnerService: SpinnerService,
    private authService: AuthenticationService
  ) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
   // this.spinnerService.show();
    return next.handle(req).pipe(
      tap(
        (event: HttpEvent<any>) => {
          if (event instanceof HttpResponse) {
           // this.spinnerService.start();
          }
        },
        (error) => {
          if (error.status === 401) {
            this.authService.logout;
          }
         // this.spinnerServicestop();
        }
      )
    );
  }
}
