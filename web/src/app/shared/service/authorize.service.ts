import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { AppConfig } from './app-config.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {

  private readonly _verifyTxnPassOtp: string = '/api/Authorization/verifyuser';
  private readonly _generateOtp: string = '/api/Authorization/generateotp';

  get verifyTxnPassOtpUrl() { return this.appConfig.settings.apiBaseUrl + this._verifyTxnPassOtp; }
  get generateOtpUrl() { return this.appConfig.settings.apiBaseUrl + this._generateOtp; }

  constructor(private router: Router,
    private http: HttpClient,
    private appConfig: AppConfig
  ) {

  }

  verifyTxnOtp(model: any) {
    const endpointUrl = `${this.verifyTxnPassOtpUrl}`;
     const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Accept': 'application/json' });
     return this.http.post<any>(endpointUrl, model, { headers: headers, withCredentials: true,  observe: "response", })
   // const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Accept': 'application/json', 'iv-user': 'dharamc-30228696', 'stepup-auth-mechansim': 'otp' });
   // return this.http.post<any>("https://localhost:5001/api/Authorization/verifyuser", model, { headers: headers, observe: "response" });
  }
  generateOtp(model: any) {
    const endpointUrl = `${this.generateOtpUrl}`;
    const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Accept': 'application/json' });
    return this.http.post<any>(endpointUrl, model, { headers: headers, withCredentials: true })
    //const headers = new HttpHeaders({'Content-Type': 'application/json', 'Accept': 'application/json','iv-user':'dharamc-30228696','stepup-auth-mechansim':'otp' });
    //return this.http.post("https://localhost:5001/api/Authorization/generateotp", model, { headers: headers });
  }

}
