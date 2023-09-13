import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ConfigurationService } from 'src/app/shared/service/configuration.service';
import { EndpointBase } from 'src/app/shared/service/endpoint-base.service';

@Injectable({
  providedIn: 'root'
})
export class ReportService extends EndpointBase {

  private get getBookingUrl() { return this.configurations.baseUrl + '/api/Report/booking'; }
  private get getDealerUrl() { return this.configurations.baseUrl + '/api/Report/dealer'; }
  private get getCityUrl() { return this.configurations.baseUrl + '/api/Report/city'; }
  private get getShowRoomUrl() { return this.configurations.baseUrl + '/api/Report/showroom'; }

  constructor(private configurations: ConfigurationService, http: HttpClient, authService: AuthenticationService) {
    super(http, authService);
  }

  getDealers() {
    return this.http.get(this.getDealerUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getDealers());
      }));
  }
  getCity() {
    return this.http.get(this.getCityUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getCity());
      }));
  }
  getShowRoom() {
    let dealerid=this.authService.currentUser.id;
    let url=`${this.getShowRoomUrl}/${dealerid}`;
    return this.http.get(url, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getShowRoom());
      }));
  }

  getBokingDetail(data:any) {
    return this.http.post(this.getBookingUrl,JSON.stringify(data) ,this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBokingDetail(data));
      }));
  }


  
}