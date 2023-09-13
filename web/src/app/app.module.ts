import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { OidcHelperService } from './shared/service/oidc-helper.service';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from '@angular/flex-layout';
import { SharedModule } from './shared/shared.module';
import { OAuthModule } from 'angular-oauth2-oidc';
import { DatePipe, CurrencyPipe } from '@angular/common';
import { AppConfig } from './shared/service/app-config.service';
import { ConfigurationService } from './shared/service/configuration.service';

import { AuthGuard } from './shared/guards/auth.guard';
export function initializeApp(appConfig: AppConfig) {
  return () => appConfig.load();
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    CoreModule,
    SharedModule,
    HttpClientModule,
    OAuthModule.forRoot(),

  ],
  providers: [
    DatePipe,
    CurrencyPipe,
    AppConfig,
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [AppConfig], multi: true
    },
    AuthenticationService,
    OidcHelperService,
    ConfigurationService,
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
