import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ConfigurationService } from 'src/app/shared/service/configuration.service';
import { EndpointBase } from 'src/app/shared/service/endpoint-base.service';

@Injectable({
  providedIn: 'root'
})
export class BookingService extends EndpointBase {

  private get getBookingUrl() { return this.configurations.baseUrl + '/api/BikeBooking/bookings'; }
  private get createBookingUrl() { return this.configurations.baseUrl + '/api/BikeBooking/bookings'; }
  private get updateBookingUrl() { return this.configurations.baseUrl + '/api/BikeBooking/bookings'; }
  private get deleteBookingUrl() { return this.configurations.baseUrl + '/api/BikeBooking/bookings'; }
  
  private get authorizeBookingUrl() { return this.configurations.baseUrl + '/api/BikeBooking/booking-status'; }
  
  
  constructor(private configurations: ConfigurationService, http: HttpClient, authService: AuthenticationService) {
    super(http, authService);
  }

  //Specification
  createBooking(obj: any) {
    return this.http.post(this.createBookingUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createBooking(obj));
      }));
  }
  getBookings(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getBookingUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBookings(page, pageSize));
      }));
  }
  updateBooking(obj: any, id?: string) {
    const endpointUrl = `${this.updateBookingUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateBooking(obj, id));
      }));
  }
  deleteBooking(id: string) {
    const endpointUrl = `${this.deleteBookingUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteBooking(id));
      }));
  }
  authorizeBooking(request) {
    const endpointUrl = `${this.authorizeBookingUrl}`
    return this.http.post(endpointUrl, JSON.stringify(request), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.authorizeBooking(request));
      }));
  }

}