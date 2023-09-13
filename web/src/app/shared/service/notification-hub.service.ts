import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';


@Injectable()
export class NotificationHubService extends EndpointBase {

  get notificationResultUrl() { return this.configurations.baseUrl + '/api/Notification/notifications'; }
  get clearNotificationsUrl() { return this.configurations.baseUrl + '/api/Notification/clearnotifications'; }
  get deleteNotificationsUrl() { return this.configurations.baseUrl + '/api/Notification/deletenotifications'; }





  constructor(private configurations: ConfigurationService, http: HttpClient, authService: AuthenticationService) {
    super(http, authService);
  }




  getNotifications(): Observable<any> {
    return this.http.get<any>(this.notificationResultUrl, this.requestHeaders).pipe<any>(
      catchError(error => {
        return this.handleError(error, () => this.getNotifications());
      }));
  }
  clearNotifications(): Observable<any> {
    return this.http.delete<any>(this.clearNotificationsUrl, this.requestHeaders).pipe<any>(
      catchError(error => {
        return this.handleError(error, () => this.clearNotifications());
      }));
  }
  deleteNotifications(id): Observable<any> {
    const endpointUrl = `${this.deleteNotificationsUrl}/${id}`;
    return this.http.delete<any>(endpointUrl, this.requestHeaders).pipe<any>(
      catchError(error => {
        return this.handleError(error, () => this.deleteNotifications(id));
      }));
  }



}
