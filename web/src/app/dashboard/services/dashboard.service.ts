import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ConfigurationService } from 'src/app/shared/service/configuration.service';
import { EndpointBase } from 'src/app/shared/service/endpoint-base.service';

@Injectable({
  providedIn: 'root'
})
export class DashBoardService extends EndpointBase {

  private get getDetailUrl() { return this.configurations.baseUrl + '/api/DashBoard/detail'; }

  constructor(private configurations: ConfigurationService, http: HttpClient, authService: AuthenticationService) {
    super(http, authService);
  }



  getDashboardDetails() {
 
    return this.http.get(this.getDetailUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getDashboardDetails());
      }));
  }
  

}