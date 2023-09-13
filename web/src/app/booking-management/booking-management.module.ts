import { BookingListComponent } from './components/booking-list/booking-list.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { BookingComponent } from './components/booking-list/booking/booking.component';
import { BikeVariantsComponent } from './components/booking-list/bike-variants/bike-variants.component';
import { BookingDetailsComponent } from './components/booking-list/booking-details/booking-details.component';

export const routes: Routes = [
  { path: 'booking', component: BookingListComponent},
  { path: 'booking-details', component: BookingDetailsComponent},
];


@NgModule({
  declarations: [BookingListComponent, BookingComponent, BikeVariantsComponent, BookingDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class BookingManagementModule { }
