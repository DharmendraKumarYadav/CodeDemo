import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ConfigurationService } from 'src/app/shared/service/configuration.service';
import { EndpointBase } from 'src/app/shared/service/endpoint-base.service';

@Injectable({
  providedIn: 'root'
})
export class LocationService extends EndpointBase {

  private get getCityUrl() { return this.configurations.baseUrl + '/api/Location/city'; }
  private get createCityUrl() { return this.configurations.baseUrl + '/api/Location/city'; }
  private get updateCityUrl() { return this.configurations.baseUrl + '/api/Location/city'; }
  private get deleteCityUrl() { return this.configurations.baseUrl + '/api/Location/city'; }

  private get getAreaUrl() { return this.configurations.baseUrl + '/api/Location/area'; }
  private get createAreaUrl() { return this.configurations.baseUrl + '/api/Location/area'; }
  private get updateAreaUrl() { return this.configurations.baseUrl + '/api/Location/area'; }
  private get deleteAreaUrl() { return this.configurations.baseUrl + '/api/Location/area'; }




  constructor(private configurations: ConfigurationService, http: HttpClient, authService: AuthenticationService) {
    super(http, authService);
  }

  //Specification
  createCity(obj: any) {
    return this.http.post(this.createCityUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createCity(obj));
      }));
  }
  getCity(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getCityUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getCity(page, pageSize));
      }));
  }
  updateCity(obj: any, id?: string) {
    const endpointUrl = `${this.updateCityUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateCity(obj, id));
      }));
  }
  deleteCity(id: string) {
    const endpointUrl = `${this.deleteCityUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteCity(id));
      }));
  }

  //Attribute
  createArea(obj: any) {
    return this.http.post(this.createAreaUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createArea(obj));
      }));
  }
  getArea(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getAreaUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getArea(page, pageSize));
      }));
  }
  updateArea(obj: any, id?: string) {
    const endpointUrl = `${this.updateAreaUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateArea(obj, id));
      }));
  }
  deleteArea(id: string) {
    const endpointUrl = `${this.deleteAreaUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteArea(id));
      }));
  }
}