import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ConfigurationService } from 'src/app/shared/service/configuration.service';
import { EndpointBase } from 'src/app/shared/service/endpoint-base.service';

@Injectable({
  providedIn: 'root'
})
export class ShowroomService extends EndpointBase {
  private get getDealerUrl() { return this.configurations.baseUrl + '/api/ShowRoom/dealers'; }
  private get getBrokerUrl() { return this.configurations.baseUrl + '/api/ShowRoom/brokers'; }
  private get getDealerBikeUrl() { return this.configurations.baseUrl + '/api/ShowRoom/dealerbikes'; }
  private get getBrokerBikeUrl() { return this.configurations.baseUrl + '/api/ShowRoom/salebikes'; }
  private get requestDealerBikeUrl() { return this.configurations.baseUrl + '/api/ShowRoom/bikerequest'; }
  private get updateBrokerBikeUrl() { return this.configurations.baseUrl + '/api/ShowRoom/salebike'; }
  private get returnDealerBikeUrl() { return this.configurations.baseUrl + '/api/ShowRoom/returnrequest'; }
  private get getBookedBikeUrl() { return this.configurations.baseUrl + '/api/ShowRoom/bookedbikes'; }

  private get getBrokerBikeRequestUrl() { return this.configurations.baseUrl + '/api/ShowRoom/brokerrequested'; }
  private get getBrokerAuthorizeRequestUrl() { return this.configurations.baseUrl + '/api/ShowRoom/authorizerequest'; }

  private get createDealerBikeUrl() { return this.configurations.baseUrl + '/api/ShowRoom/dealerbike'; }
  private get updateDealerBikeUrl() { return this.configurations.baseUrl + '/api/ShowRoom/dealerbike'; }
  private get deleteDealerBikeUrl() { return this.configurations.baseUrl + '/api/ShowRoom/dealerbike'; }

  private get getShowRoomUrl() { return this.configurations.baseUrl + '/api/ShowRoom/showroomsapproved'; }
  private get getAdminShowRoomUrl() { return this.configurations.baseUrl + '/api/ShowRoom/showrooms'; }
  private get createShowRoomUrl() { return this.configurations.baseUrl + '/api/ShowRoom/showroom'; }
  private get updateShowRoomUrl() { return this.configurations.baseUrl + '/api/ShowRoom/showroom'; }
  private get deleteShowRoomUrl() { return this.configurations.baseUrl + '/api/ShowRoom/showroom'; }
  private get getShowRoomAuthorizeRequestUrl() { return this.configurations.baseUrl + '/api/ShowRoom/showroomauthorize'; }
  


  constructor(private configurations: ConfigurationService, http: HttpClient, authService: AuthenticationService) {
    super(http, authService);
  }

  getDealers() {
    const endpointUrl = `${this.getDealerUrl}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getDealers());
      }));
  }
  getBrokers() {
    const endpointUrl = `${this.getBrokerUrl}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getDealers());
      }));
  }

  getDealerBikeByUserId(dealerId) {
    const endpointUrl = `${this.getDealerBikeUrl}/${dealerId}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this. getDealerBikeByUserId(dealerId));
      }));
  }
  getBrokerSaleBike(userId:string,page?: number, pageSize?: number) {
    const endpointUrl = `${this.getBrokerBikeUrl}/${userId}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this. getBrokerSaleBike(userId,page, pageSize));
      }));
  }
  requestDealerBike(id: string) {
    const endpointUrl = `${this.requestDealerBikeUrl}/${id}`;

    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.requestDealerBike(id));
      }));
  }
  returnDealerBike(id: string) {
    const endpointUrl = `${this.returnDealerBikeUrl}/${id}`;

    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.returnDealerBike(id));
      }));
  }

  updateBrokerBike(obj: any, id?: string) {
    
    const endpointUrl = `${this.updateBrokerBikeUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateBrokerBike(obj, id));
      }));
  }



  createDealerBike(obj: any) {
    return this.http.post(this.createDealerBikeUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createDealerBike(obj));
      }));
  }
  getDealerBike(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getDealerBikeUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getDealerBike(page, pageSize));
      }));
  }
  getBrokerBikeRequest(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getBrokerBikeRequestUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBrokerBikeRequest(page, pageSize));
      }));
  }
  authorizeBrokerRequest(request) {
    const endpointUrl = `${this.getBrokerAuthorizeRequestUrl}`
    return this.http.post(endpointUrl, JSON.stringify(request), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.authorizeBrokerRequest(request));
      }));
  }
  updateDealerBike(obj: any, id?: string) {
    const endpointUrl = `${this.updateDealerBikeUrl}/${id}`
    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateDealerBike(obj, id));
      }));
  }
  deleteDealerBike(id: string) {
    const endpointUrl = `${this.deleteDealerBikeUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteDealerBike(id));
      }));
  }
  getBookedBike(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getBookedBikeUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this. getBookedBike(page, pageSize));
      }));
  }



//ShowRoom Data:


  
  createShowRoom(obj: any) {
    obj.dealerId=this.authService.currentUser.id;
    return this.http.post(this.createShowRoomUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createShowRoom(obj));
      }));
  }
  getShowRooms(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getShowRoomUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getShowRooms(page, pageSize));
      }));
  }
  getAllShowRooms(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getAdminShowRoomUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getShowRooms(page, pageSize));
      }));
  }
  updateShowRoom(obj: any, id?: string) {
    const endpointUrl = `${this.updateShowRoomUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateShowRoom(obj, id));
      }));
  }
  deleteShowRoom(id: string) {
    const endpointUrl = `${this.deleteShowRoomUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteShowRoom(id));
      }));
  }
  authorizeShowRoomRequest(request) {
    const endpointUrl = `${this.getShowRoomAuthorizeRequestUrl}`
    return this.http.post(endpointUrl, JSON.stringify(request), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.authorizeShowRoomRequest(request));
      }));
  }

  

  }
