import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { DealersComponent } from './components/dealers/dealers.component';
import { SharedModule } from '../shared/shared.module';
import { BrokersComponent } from './components/brokers/brokers.component';
import { ShowroomsComponent } from './components/showrooms/showrooms.component';
import { ShowroomsAddUpdateComponent } from './components/showrooms/showrooms-add-update/showrooms-add-update.component';
import { DealerSaleBikeComponent } from './components/dealers/dealer-sale-bike/dealer-sale-bike.component';
import { DealerSaleBikeAddUpdateComponent } from './components/dealers/dealer-sale-bike-add-update/dealer-sale-bike-add-update.component';
import { DealerSaleBikeListViewComponent } from './components/dealers/dealer-sale-bike-list-view/dealer-sale-bike-list-view.component';
import { DealerSaleBikeAuthRequestComponent } from './components/dealers/dealer-sale-bike-auth-request/dealer-sale-bike-auth-request.component';
import { BrokerBikeSalesComponent } from './components/brokers/broker-bike-sales/broker-bike-sales.component';
import { BrokerBikeSalesAddUpdateComponent } from './components/brokers/broker-bike-sales-add-update/broker-bike-sales-add-update.component';
import { BrokerBikeSalesListViewComponent } from './components/brokers/broker-bike-sales-list-view/broker-bike-sales-list-view.component';
import { DealerAvailableBikesComponent } from './components/dealers/dealer-available-bikes/dealer-available-bikes.component';
import { BookedBikesComponent } from './components/booked-bikes/booked-bikes.component';


export const routes: Routes = [
  { path: 'dealer-bikes', component: DealerSaleBikeComponent },
  { path: 'showromms', component: ShowroomsComponent },
  { path: 'auth-requests', component: DealerSaleBikeAuthRequestComponent },
  { path: 'brokers', component: BrokersComponent },
  { path: 'dealers', component: DealersComponent },
  { path: 'bike-sale', component: BrokerBikeSalesComponent },
  { path: 'booked-bikes', component: BookedBikesComponent }
];

@NgModule({
  declarations: [DealersComponent,DealersComponent, BrokersComponent, ShowroomsComponent, ShowroomsAddUpdateComponent, DealerSaleBikeComponent, DealerSaleBikeAddUpdateComponent, DealerSaleBikeListViewComponent, DealerSaleBikeAuthRequestComponent, BrokerBikeSalesComponent, BrokerBikeSalesAddUpdateComponent, BrokerBikeSalesListViewComponent, DealerAvailableBikesComponent,BookedBikesComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class ShowroomManagementModule { }
