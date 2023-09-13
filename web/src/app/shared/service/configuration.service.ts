import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { LocalStoreManager } from './local-store-manager.service';
import { DBKeys } from './db-key';
import { Utilities } from './utilities';
import { environment } from '../../../environments/environment';
import { AppConfig } from './app-config.service';

interface UserConfiguration {
  homeUrl: string
}

@Injectable()
export class ConfigurationService {
  constructor(
    private localStorage: LocalStoreManager,public appConfig:AppConfig) {
  }

  public static readonly appVersion: string = '1.0.0';
  public static readonly defaultHomeUrl: string = '/';

  public baseUrl = this.appConfig.settings.apiBaseUrl;
  public loginUrl = "";

  private saveToLocalStore(data: any, key: string) {
    setTimeout(() => this.localStorage.savePermanentData(data, key));
  }

  public import(jsonValue: string) {
    this.clearLocalChanges();

    if (jsonValue) {
      const importValue: UserConfiguration = Utilities.JsonTryParse(jsonValue);
    }
  }

  public clearLocalChanges() {
    this.clearUserConfigKeys();
  }

  private addKeyToUserConfigKeys(configKey: string) {
    const configKeys = this.localStorage.getDataObject<string[]>(DBKeys.USER_CONFIG_KEYS);

    if (!configKeys.includes(configKey)) {
      configKeys.push(configKey);
      this.localStorage.savePermanentData(configKeys, DBKeys.USER_CONFIG_KEYS);
    }
  }
  private clearUserConfigKeys() {
    const configKeys = this.localStorage.getDataObject<string[]>(DBKeys.USER_CONFIG_KEYS);

    if (configKeys != null && configKeys.length > 0) {
      for (let key of configKeys) {
        this.localStorage.deleteData(key);
      }

      this.localStorage.deleteData(DBKeys.USER_CONFIG_KEYS);
    }
  }

  public saveConfiguration(data: any, configKey: string) {
    this.addKeyToUserConfigKeys(configKey);
    this.localStorage.savePermanentData(data, configKey)
  }

  public getConfiguration<T>(configKey: string, isDateType = false): T {
    return this.localStorage.getDataObject<T>(configKey, isDateType);
  }
}
