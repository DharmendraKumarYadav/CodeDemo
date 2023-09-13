import { BikeGeneralDetailComponent } from './components/bike-list/bike/bike-general-detail/bike-general-detail.component';
import { BikeComponent } from './components/bike-list/bike/bike.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BikeListComponent } from './components/bike-list/bike-list.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';
import { AccountService } from '../shared/service/account.service';
import { AccountEndpoint } from '../shared/service/account-endpoint.service';
import { BikePhotoUploadComponent } from './components/bike-list/bike/bike-photo/bike-photo-upload/bike-photo-upload.component';
import { BikeFeaturesComponent } from './components/bike-list/bike/bike-features-list/bike-features/bike-features.component';
import { BikeFeaturesListComponent } from './components/bike-list/bike/bike-features-list/bike-features-list.component';
import { RelatedBikeComponent } from './components/bike-list/bike/related-bike/related-bike.component';

export const routes: Routes = [
  { path: 'bikes', component: BikeListComponent},
  { path: 'bike', component: BikeComponent},
  { path: 'bike-features', component: BikeFeaturesListComponent},
  { path: 'bike/:id', component: BikeComponent}
]

@NgModule({
  declarations: [BikeListComponent,BikeComponent,BikeGeneralDetailComponent, BikePhotoUploadComponent, BikeFeaturesComponent, BikeFeaturesListComponent, RelatedBikeComponent],
  providers:[AccountService,AccountEndpoint ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class BikeManagementModule { }
