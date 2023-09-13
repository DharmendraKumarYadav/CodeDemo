import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ConfigurationService } from 'src/app/shared/service/configuration.service';
import { EndpointBase } from 'src/app/shared/service/endpoint-base.service';

@Injectable({
  providedIn: 'root'
})
export class BikeService extends EndpointBase {

  private get getBikesUrl() { return this.configurations.baseUrl + '/api/Bike/bikes'; }
  private get getBikeGeneralDetailsByIdUrl() { return this.configurations.baseUrl + '/api/Bike/bike'; }
  private get saveGeneralDetailsUrl() { return this.configurations.baseUrl + '/api/Bike/bike'; }
  private get deleteBikeUrl() { return this.configurations.baseUrl + '/api/Bike/bike'; }
  private get deleteBikeVariantsUrl() { return this.configurations.baseUrl + '/api/Bike/bike-variants'; }
  private get deleteBikeBroucherUrl() { return this.configurations.baseUrl + '/api/Bike/bike-broucher'; }

  
  private get getBikePhotoUrl() { return this.configurations.baseUrl + '/api/Bike/bike-image'; }
  private get savePhtotUrl() { return this.configurations.baseUrl + '/api/Bike/bike-image'; }
  private get deleteBikePhotoUrl() { return this.configurations.baseUrl + '/api/Bike/bike-image'; }

  private get getBikePriceUrl() { return this.configurations.baseUrl + '/api/Bike/bike-price'; }
  private get saveBikePriceUrl() { return this.configurations.baseUrl + '/api/Bike/bike-price'; }
  private get deleteBikePriceUrl() { return this.configurations.baseUrl + '/api/Bike/bike-price'; }

  private get getBikeSpecificationUrl() { return this.configurations.baseUrl + '/api/Bike/bike-specification'; }
  private get saveBikeSpecificationUrl() { return this.configurations.baseUrl + '/api/Bike/bike-specification'; }
  private get deleteBikeSpecificationUrl() { return this.configurations.baseUrl + '/api/Bike/bike-specification'; }

  private get getBikeFeaturesUrl() { return this.configurations.baseUrl + '/api/Bike/bike-features'; }
  private get saveBikeFeaturesUrl() { return this.configurations.baseUrl + '/api/Bike/bike-features'; }
  private get deleteBikeFeaturesUrl() { return this.configurations.baseUrl + '/api/Bike/bike-features'; }

  private get getFeatureBikeTypeUrl() { return this.configurations.baseUrl + '/api/Bike/feature-bike-type'; }
  private get saveFeatureBikeTypeUrl() { return this.configurations.baseUrl + '/api/Bike/feature-bike-type'; }
  private get updateFeatureBikeTypeUrl() { return this.configurations.baseUrl + '/api/Bike/feature-bike-type'; }
  private get deleteFeatureBikeTypeUrl() { return this.configurations.baseUrl + '/api/Bike/feature-bike-type'; }

  private get getBikeVariantPriceUrl() { return this.configurations.baseUrl + '/api/Bike/bike-details'; }

  private get getRelatedBikeUrl() { return this.configurations.baseUrl + '/api/Bike/bike-related'; }
  private get saveRelatedtUrl() { return this.configurations.baseUrl + '/api/Bike/bike-related'; }
  private get deleteRelatedBikeUrl() { return this.configurations.baseUrl + '/api/Bike/bike-related'; }


  private get getBikeVariantPriceByCityUrl() { return this.configurations.baseUrl + '/api/Bike/bikebycity'; }


  constructor(private configurations: ConfigurationService, http: HttpClient, authService: AuthenticationService) {
    super(http, authService);
  }

  getBikes(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getBikesUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBikes(page, pageSize));
      }));
  }
  getBikeVariantPrice(bikeId?: number, cityId?: number) {
    const endpointUrl = `${this.getBikeVariantPriceUrl}/${bikeId}/${cityId}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBikeVariantPrice(bikeId, cityId));
      }));
  }

  getBikeVariantPriceByCity(showroomid?: number) {
    const endpointUrl = `${this.getBikeVariantPriceByCityUrl}/${showroomid}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBikeVariantPrice(showroomid));
      }));
  }

  saveBikeGenearlDetail(obj: any) {
    return this.http.post(this.saveGeneralDetailsUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.saveBikeGenearlDetail(obj));
      }));
  }
  getBikeGenearlDetail(bikeId: number) {
    const endpointUrl = `${this.getBikeGeneralDetailsByIdUrl}/${bikeId}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBikeGenearlDetail(bikeId));
      }));
  }
  deleteBikeVariants(id: string) {
    const endpointUrl = `${this.deleteBikeVariantsUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteBikeVariants(id));
      }));
  }
  deleteBikeBroucher(id: string) {
    const endpointUrl = `${this.deleteBikeBroucherUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteBikeBroucher(id));
      }));
  }

  getBikePhotos(bikeId: number) {
    const endpointUrl = `${this.getBikePhotoUrl}/${bikeId}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBikePhotos(bikeId));
      }));
  }
  saveBikePhoto(obj: any) {
    return this.http.post(this.savePhtotUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.saveBikePhoto(obj));
      }));
  }
  deleteBikePhoto(id: string) {
    const endpointUrl = `${this.deleteBikePhotoUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteBikePhoto(id));
      }));
  }

  
  getBikePrice(bikeId: number) {
    const endpointUrl = `${this.getBikePriceUrl}/${bikeId}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBikePrice(bikeId));
      }));
  }
  saveBikePrice(obj: any) {
    return this.http.post(this.saveBikePriceUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.saveBikePrice(obj));
      }));
  }
  deleteBikePrice(id: string) {
    const endpointUrl = `${this.deleteBikePriceUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteBikePrice(id));
      }));
  }


  getBikeSpecification(bikeId: number) {
    const endpointUrl = `${this.getBikeSpecificationUrl}/${bikeId}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBikePrice(bikeId));
      }));
  }
  saveBikeSpecification(obj: any) {
    return this.http.post(this.saveBikeSpecificationUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.saveBikeSpecification(obj));
      }));
  }
  deleteBikeSpecification(id: string) {
    const endpointUrl = `${this.deleteBikeSpecificationUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteBikeSpecification(id));
      }));
  }


  getBikeFeatures(bikeId: number) {
    const endpointUrl = `${this.getBikeFeaturesUrl}/${bikeId}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBikeFeatures(bikeId));
      }));
  }
  saveBikeFeatures(obj: any) {
    return this.http.post(this.saveBikeFeaturesUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.saveBikeFeatures(obj));
      }));
  }
  deleteBikeFeatures(id: string) {
    const endpointUrl = `${this.deleteBikeFeaturesUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteBikeFeatures(id));
      }));
  }

  
  getFeatureBikeType(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getFeatureBikeTypeUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getFeatureBikeType());
      }));
  }
  saveFeatureBikeType(obj: any) {
    return this.http.post(this.saveFeatureBikeTypeUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.saveBikeFeatures(obj));
      }));
  }

  updateFeatureBikeType(obj: any,id:number) {
    const endpointUrl = `${this.updateFeatureBikeTypeUrl}/${id}`
    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateFeatureBikeType(obj,id));
      }));
  }
  deleteFeatureBikeType(id: string) {
    const endpointUrl = `${this.deleteFeatureBikeTypeUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteFeatureBikeType(id));
      }));
  }

  getRelatedBikes(bikeId: number) {
    let page=-1;
    let pageSize=-1;
    const endpointUrl = `${this.getRelatedBikeUrl}/${page}/${pageSize}/${bikeId}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getRelatedBikes(bikeId));
      }));
  }
  saveBikeRelated(obj: any) {
    return this.http.post(this.saveRelatedtUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.saveBikeRelated(obj));
      }));
  }
  deleteBikeRelated(id: string) {
    const endpointUrl = `${this.deleteRelatedBikeUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteBikeRelated(id));
      }));
  }

  deleteBike(id: string) {
    const endpointUrl = `${this.deleteBikeUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteBike(id));
      }));
  }
}
