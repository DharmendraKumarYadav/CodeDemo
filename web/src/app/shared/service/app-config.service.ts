import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IAppConfig } from 'src/app/shared/model/common/app-config.model';
import { Subject } from 'rxjs';
import { LocalStoreManager } from './local-store-manager.service';
import { DBKeys } from './db-key';
@Injectable()
export class AppConfig {
    public settings: IAppConfig;
    private configChange$ = new Subject<IAppConfig>();
    public config$ = this.configChange$.asObservable();

    constructor(private http: HttpClient, private localStorage: LocalStoreManager) { }
    load() {
        return new Promise<void>((resolve, reject) => {
            this.http.get('assets/config/app.config.json').toPromise().then((response: IAppConfig) => {
                this.configChange$.next(<IAppConfig>response);
                this.settings = <IAppConfig>response;
                this.localStorage.saveSyncedSessionData(this.settings, DBKeys.CONFIG_DATA);
                resolve();
            }).catch((response: any) => {
                reject("Unable to load app.");
            });
        });
    }
}