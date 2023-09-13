import { DashboardRoutingModule } from './dashboard-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { DashboardViewComponent } from './component/dashboard-view/dashboard-view.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { LayoutModule } from '@angular/cdk/layout';
import { CardComponent } from './component/card/card.component';
import { BookingComponent } from './component/charts/booking/booking.component';
// import { DealerComponent } from './component/charts/dealer/dealer.component';
import { CustomerComponent } from './component/charts/customer/customer.component';
import { BikesComponent } from './component/charts/bikes/bikes.component';
import { MiniCardComponent } from './component/mini-card/mini-card.component';



@NgModule({
  declarations: [  DashboardViewComponent, CardComponent, BookingComponent, CustomerComponent, BikesComponent, MiniCardComponent],
  imports: [
    CommonModule,
    SharedModule,
    DashboardRoutingModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    LayoutModule
  ]
})
export class DashboardModule { }
