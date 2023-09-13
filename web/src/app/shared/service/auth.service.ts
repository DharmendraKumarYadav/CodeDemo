import { LoginModel } from "../model/auth/login.model";
import { Injectable } from "@angular/core";
import { map, switchMap } from "rxjs/operators";
import { Router } from "@angular/router";
import { LocalStoreManager } from "./local-store-manager.service";
import { User } from "src/app/shared/model/auth/user.model";
import { DBKeys } from "./db-key";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { UserInfoResponse } from "src/app/shared/model/auth/user-info.model";
import { AppConfig } from "./app-config.service";
import { MenuName } from "src/app/shared/model/common/nav-item";
import { IAppConfig } from "src/app/shared/model/common/app-config.model";
import { OidcHelperService } from "./oidc-helper.service";
import { ConfigurationService } from "./configuration.service";
import { AccessToken, LoginResponse } from "src/app/shared/model/auth/login-response.model";
import { PermissionValues } from "src/app/shared/model/auth/permission.model";
import { JwtHelper } from "./jwt-helper";
class AppRole {
  public static Adminstrator = "administrator";
  public static Dealer = "dealer";
  public static Broker = "broker";
}
@Injectable({
  providedIn: "root",
})
export class AuthenticationService {
  public loginRedirectUrl: string;
  public logoutRedirectUrl: string;
  public reLoginDelegate: () => void;

  constructor(
    private router: Router,
    private localStorage: LocalStoreManager,
    private http: HttpClient,
    private appConfig: AppConfig,
    private oidcHelperService: OidcHelperService,
    private configurations: ConfigurationService,
  ) {

  }
  reLogin() {
    if (this.reLoginDelegate) {
      this.reLoginDelegate();
    } else {
      this.redirectForLogin();
    }
  }
  redirectForLogin() {
    this.loginRedirectUrl = this.router.url;
    this.router.navigate(['/auth']);
  }
  refreshLogin() {
    return this.oidcHelperService.refreshLogin()
      .pipe(map(resp => this.processLoginResponse(resp, this.rememberMe)));
  }
  loginWithPassword(userName: string, password: string, rememberMe?: boolean) {
    if (this.isLoggedIn) {
      this.logout();
    }

    return this.oidcHelperService.loginWithPassword(userName, password)
      .pipe(map(resp => this.processLoginResponse(resp, rememberMe)));
  }

  private processLoginResponse(response: LoginResponse, rememberMe?: boolean) {
    const accessToken = response.access_token;

    if (accessToken == null) {
      throw new Error('accessToken cannot be null');
    }

    rememberMe = rememberMe || this.rememberMe;

    const refreshToken = response.refresh_token || this.refreshToken;
    const expiresIn = response.expires_in;
    const tokenExpiryDate = new Date();
    tokenExpiryDate.setSeconds(tokenExpiryDate.getSeconds() + expiresIn);
    const accessTokenExpiry = tokenExpiryDate;
    const jwtHelper = new JwtHelper();
    const decodedAccessToken = jwtHelper.decodeToken(accessToken) as AccessToken;
  if (!this.isLoggedIn) {
      this.configurations.import(decodedAccessToken.configuration);
    }

    const user = new User(
      decodedAccessToken.sub,
      decodedAccessToken.name,
      decodedAccessToken.fullname,
      decodedAccessToken.email,
      decodedAccessToken.phone_number,
      decodedAccessToken.role)
    user.isEnabled = true;

    this.saveUserDetails(user, accessToken, refreshToken, accessTokenExpiry, rememberMe);

    // this.reevaluateLoginStatus(user);

    return user;
  }

  private saveUserDetails(user: User,accessToken: string, refreshToken: string, expiresIn: Date, rememberMe: boolean) {
    if (rememberMe) {
      this.localStorage.savePermanentData(accessToken, DBKeys.ACCESS_TOKEN);
      this.localStorage.savePermanentData(refreshToken, DBKeys.REFRESH_TOKEN);
      this.localStorage.savePermanentData(expiresIn, DBKeys.TOKEN_EXPIRES_IN);
      this.localStorage.savePermanentData(user, DBKeys.CURRENT_USER);
    } else {
      this.localStorage.saveSyncedSessionData(accessToken, DBKeys.ACCESS_TOKEN);
      this.localStorage.saveSyncedSessionData(refreshToken, DBKeys.REFRESH_TOKEN);
      this.localStorage.saveSyncedSessionData(expiresIn, DBKeys.TOKEN_EXPIRES_IN);
      this.localStorage.saveSyncedSessionData(user, DBKeys.CURRENT_USER);
    }

    this.localStorage.savePermanentData(rememberMe, DBKeys.REMEMBER_ME);
  }

  logout(): void {
    this.localStorage.deleteData(DBKeys.ACCESS_TOKEN);
    this.localStorage.deleteData(DBKeys.REFRESH_TOKEN);
    this.localStorage.deleteData(DBKeys.TOKEN_EXPIRES_IN);
    this.localStorage.deleteData(DBKeys.CURRENT_USER);
    this.configurations.clearLocalChanges();

    this.redirectForLogin();
  }

  get currentUser(): User {
    const user = this.localStorage.getDataObject<User>(DBKeys.CURRENT_USER);
    return user;
  }

  get isAdminstrator(): boolean {
    const user = this.localStorage.getDataObject<User>(DBKeys.CURRENT_USER);
    if (user.role == AppRole.Adminstrator) {
      return true;
    } else {
    } return false;

  }
  
  get isDealer(): boolean {
    const user = this.localStorage.getDataObject<User>(DBKeys.CURRENT_USER);
    if (user.role == AppRole.Dealer) {
      return true;
    } else {
    } return false;

  }
  get isBroker(): boolean {
    const user = this.localStorage.getDataObject<User>(DBKeys.CURRENT_USER);
    if (user.role == AppRole.Broker) {
      return true;
    } else {
    } return false;

  }


  get accessToken(): string {
    return this.oidcHelperService.accessToken;
  }

  get accessTokenExpiryDate(): Date {
    return this.oidcHelperService.accessTokenExpiryDate;
  }

  get refreshToken(): string {
    return this.oidcHelperService.refreshToken;
  }

  get isSessionExpired(): boolean {
    return this.oidcHelperService.isSessionExpired;
  }

  get isLoggedIn(): boolean {
    return this.currentUser != null;
  }

  get rememberMe(): boolean {
    return this.localStorage.getDataObject<boolean>(DBKeys.REMEMBER_ME) === true;
  }

}
