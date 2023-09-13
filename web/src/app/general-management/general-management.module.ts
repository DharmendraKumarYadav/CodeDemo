import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BikeRatingsComponent } from './components/bike-ratings/bike-ratings.component';
import { UserBikeRequestComponent } from './components/user-bike-request/user-bike-request.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';

export const routes: Routes = [
  { path: 'bike-rating', component: BikeRatingsComponent},
  { path: 'bike-request', component: UserBikeRequestComponent},
];


@NgModule({
  declarations: [BikeRatingsComponent, UserBikeRequestComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class GeneralManagementModule { }
