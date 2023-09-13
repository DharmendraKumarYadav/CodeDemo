import { AreaComponent } from './components/area-list/area/area.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CityListComponent } from './components/city-list/city-list.component';
import { CityComponent } from './components/city-list/city/city.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';
import { AreaListComponent } from './components/area-list/area-list.component';

export const routes: Routes = [
  { path: 'city', component: CityListComponent},
  { path: 'area', component: AreaListComponent }
];

@NgModule({
  declarations: [CityListComponent, CityComponent,AreaListComponent,AreaComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class LocationManagementModule { }
