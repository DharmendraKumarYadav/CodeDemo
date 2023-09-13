import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ConfigurationService } from 'src/app/shared/service/configuration.service';
import { EndpointBase } from 'src/app/shared/service/endpoint-base.service';

@Injectable({
  providedIn: 'root'
})
export class BikeSpecificationService extends EndpointBase {

  private get getSpecificationUrl() { return this.configurations.baseUrl + '/api/Specification/specification'; }
  private get createSpecificationUrl() { return this.configurations.baseUrl + '/api/Specification/specification'; }
  private get updateSpecificationUrl() { return this.configurations.baseUrl + '/api/Specification/specification'; }
  private get deleteSpecificationUrl() { return this.configurations.baseUrl + '/api/Specification/specification'; }

  private get getAttributeUrl() { return this.configurations.baseUrl + '/api/Attribute/attribute'; }
  private get createAttributeUrl() { return this.configurations.baseUrl + '/api/Attribute/attributes'; }
  private get updateAttributeUrl() { return this.configurations.baseUrl + '/api/Attribute/attributes'; }
  private get deleteAttributeUrl() { return this.configurations.baseUrl + '/api/Attribute/attribute'; }

  private get getBrandUrl() { return this.configurations.baseUrl + '/api/brand/brand'; }
  private get createBrandUrl() { return this.configurations.baseUrl + '/api/brand/brand'; }
  private get updateBrandUrl() { return this.configurations.baseUrl + '/api/brand/brand'; }
  private get deleteBrandUrl() { return this.configurations.baseUrl + '/api/brand/brand'; }

  private get getColourUrl() { return this.configurations.baseUrl + '/api/colour/colour'; }
  private get createColourUrl() { return this.configurations.baseUrl + '/api/colour/colour'; }
  private get updateColourUrl() { return this.configurations.baseUrl + '/api/colour/colour'; }
  private get deleteColourUrl() { return this.configurations.baseUrl + '/api/colour/colour'; }

  private get getCategoryUrl() { return this.configurations.baseUrl + '/api/category/category'; }
  private get createCategoryUrl() { return this.configurations.baseUrl + '/api/Category/category'; }
  private get updateCategoryUrl() { return this.configurations.baseUrl + '/api/category/category'; }
  private get deleteCategoryUrl() { return this.configurations.baseUrl + '/api/category/category'; }

  private get getDisplacementUrl() { return this.configurations.baseUrl +  '/api/displacement/displacement'; }
  private get createDisplacementUrl() { return this.configurations.baseUrl + '/api/displacement/displacement'; }
  private get updateDisplacementUrl() { return this.configurations.baseUrl + '/api/displacement/displacement'; }
  private get deleteDisplacementUrl() { return this.configurations.baseUrl + '/api/displacement/displacement'; }

  private  get getBudgetUrl() { return this.configurations.baseUrl +  '/api/budget/budget'; }
  private get createBudgetUrl() { return this.configurations.baseUrl + '/api/budget/budget'; }
  private get updateBudgetUrl() { return this.configurations.baseUrl + '/api/budget/budget'; }
  private get deleteBudgetUrl() { return this.configurations.baseUrl + '/api/budget/budget'; }

  private get getBodyStyleUrl() { return this.configurations.baseUrl +  '/api/bodystyle/bodystyle'; }
  private get createBodyStyleUrl() { return this.configurations.baseUrl + '/api/bodystyle/bodystyle'; }
  private get updateBodyStyleUrl() { return this.configurations.baseUrl + '/api/bodystyle/bodystyle'; }
  private get deleteBodyStyleUrl() { return this.configurations.baseUrl + '/api/bodystyle/bodystyle'; }



  constructor(private configurations: ConfigurationService, http: HttpClient, authService: AuthenticationService) {
    super(http, authService);
  }

  //Specification
  createSpecification(obj: any) {
    return this.http.post(this.createSpecificationUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createSpecification(obj));
      }));
  }
  getSpecification(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getSpecificationUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getSpecification(page, pageSize));
      }));
  }
  updateSpecification(obj: any, id?: string) {
    const endpointUrl = `${this.updateSpecificationUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateSpecification(obj, id));
      }));
  }
  deleteSpecification(id: string) {
    const endpointUrl = `${this.deleteSpecificationUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteSpecification(id));
      }));
  }

  //Attribute
  createAttribute(obj: any) {
    return this.http.post(this.createAttributeUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createAttribute(obj));
      }));
  }
  getAttribute(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getAttributeUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getAttribute(page, pageSize));
      }));
  }
  updateAttribute(obj: any, id?: string) {
    const endpointUrl = `${this.updateAttributeUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateAttribute(obj, id));
      }));
  }
  deleteAttribute(id: string) {
    const endpointUrl = `${this.deleteAttributeUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteAttribute(id));
      }));
  }

  //Brand
  createBrand(obj: any) {
    return this.http.post(this.createBrandUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createBrand(obj));
      }));
  }
  getBrand(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getBrandUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBrand(page, pageSize));
      }));
  }
  updateBrand(obj: any, id?: string) {
    const endpointUrl = `${this.updateBrandUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateBrand(obj, id));
      }));
  }
  deleteBrand(id: string) {
    const endpointUrl = `${this.deleteBrandUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteBrand(id));
      }));
  }
  //Colour
  createColour(obj: any) {
    return this.http.post(this.createColourUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createColour(obj));
      }));
  }
  getColour(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getColourUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getColour(page, pageSize));
      }));
  }
  updateColour(obj: any, id?: string) {
    const endpointUrl = `${this.updateColourUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateColour(obj, id));
      }));
  }
  deleteColour(id: string) {
    const endpointUrl = `${this.deleteColourUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteColour(id));
      }));
  }

  //Category
  createCategory(obj: any) {
    return this.http.post(this.createCategoryUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createCategory(obj));
      }));
  }
  getCategory(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getCategoryUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getCategory(page, pageSize));
      }));
  }
  updateCategory(obj: any, id?: string) {
    const endpointUrl = `${this.updateCategoryUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateCategory(obj, id));
      }));
  }
  deleteCategory(id: string) {
    const endpointUrl = `${this.deleteCategoryUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteCategory(id));
      }));
  }

  //Displacement
  createDisplacement(obj: any) {
    return this.http.post(this.createDisplacementUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createDisplacement(obj));
      }));
  }
  getDisplacement(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getDisplacementUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getDisplacement(page, pageSize));
      }));
  }
  updateDisplacement(obj: any, id?: string) {
    const endpointUrl = `${this.updateDisplacementUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateDisplacement(obj, id));
      }));
  }
  deleteDisplacement(id: string) {
    const endpointUrl = `${this.deleteDisplacementUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteDisplacement(id));
      }));
  }

  //Budget
  createBudget(obj: any) {
    return this.http.post(this.createBudgetUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createBudget(obj));
      }));
  }
  getBudget(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getBudgetUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBudget(page, pageSize));
      }));
  }
  updateBudget(obj: any, id?: string) {
    const endpointUrl = `${this.updateBudgetUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateBudget(obj, id));
      }));
  }
  deleteBudget(id: string) {
    const endpointUrl = `${this.deleteBudgetUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteBudget(id));
      }));
  }

  //BodyStyle
  createBodyStyle(obj: any) {
    return this.http.post(this.createBodyStyleUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.createBodyStyle(obj));
      }));
  }
  getBodyStyle(page?: number, pageSize?: number) {
    const endpointUrl = `${this.getBodyStyleUrl}/${page}/${pageSize}`;
    return this.http.get(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getBodyStyle(page, pageSize));
      }));
  }
  updateBodyStyle(obj: any, id?: string) {
    const endpointUrl = `${this.updateBodyStyleUrl}/${id}`

    return this.http.put(endpointUrl, JSON.stringify(obj), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.updateBodyStyle(obj, id));
      }));
  }
  deleteBodyStyle(id: string) {
    const endpointUrl = `${this.deleteBodyStyleUrl}/${id}`;

    return this.http.delete(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.deleteBodyStyle(id));
      }));
  }
}
