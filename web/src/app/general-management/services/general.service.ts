import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ConfigurationService } from 'src/app/shared/service/configuration.service';
import { EndpointBase } from 'src/app/shared/service/endpoint-base.service';

@Injectable({
  providedIn: 'root'
})
export class GeneralService extends EndpointBase {

  
  private get getUserBikeRequestUrl() { return this.configurations.baseUrl + '/api/Bike/bikerequest'; }
  private get getUserBikeRatingUrl() { return this.configurations.baseUrl + '/api/Bike/bikerating'; }
  private get updateRatingUrl() { return this.configurations.baseUrl + '/api/Bike/bikeratingpublished'; }


  constructor(private configurations: ConfigurationService, http: HttpClient, authService: AuthenticationService) {
    super(http, authService);
  }

  getBikeUserRequest(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getUserBikeRequestUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBikeUserRequest(page, pageSize));
      }));
  }
  getBikeUserRating(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getUserBikeRatingUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBikeUserRating(page, pageSize));
      }));
  }
  updateRating(id?: string) {
    const endpointUrl = `${this.updateRatingUrl}/${id}`
    return this.http.get(endpointUrl,  this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateRating(id));
      }));
  }


}