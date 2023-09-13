import { SharedModule } from './../shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookingReportComponent } from './components/booking-report/booking-report.component';

import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';

import { RouterModule, Routes } from '@angular/router';

export const routes: Routes = [
  { path: 'booking', component: BookingReportComponent}
];


@NgModule({
  declarations: [BookingReportComponent],
  imports: [
    CommonModule,
    SharedModule,
    MatGridListModule,
    MatCardModule,
    MatIconModule,
    RouterModule.forChild(routes)
  ]
})
export class ReportManagementModule { }
