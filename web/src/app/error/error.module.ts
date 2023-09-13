import { ErrorRoutingModule } from './error-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageErrorComponent } from './page-error/page-error.component';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [PageErrorComponent],
  imports: [
    CommonModule,
    SharedModule,
    ErrorRoutingModule,
  ]
})
export class ErrorModule { }
