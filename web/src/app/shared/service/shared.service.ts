import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  private _form: any;
  private _firstTimeData: any;
  private _rsaData: any;

  setForm(item: any) {
    this._form = item;
  }
 

  getForm(): any {
    return this._form;
  }
  resetMobile(): any {
    this._form = null;
  }

  setLoginData(item: any) {
    this._firstTimeData = item;
  }
  getLoginData(): any {
    return this._firstTimeData;
  }

  setRSAData(item: any) {
    this._rsaData = item;
  }
  getRSAData(): any {
    return this._rsaData;
  }



}
