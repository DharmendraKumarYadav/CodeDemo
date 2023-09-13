import { SharedModule } from './../shared/shared.module';
import { ChnagePasswordComponent } from './component/chnage-password/chnage-password.component';
import { PersonalDetailComponent } from './component/personal-detail/personal-detail.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutingModule } from './account-routing.module';



@NgModule({
  declarations: [PersonalDetailComponent,ChnagePasswordComponent],
 
  imports: [
    CommonModule,
    SharedModule,
    AccountRoutingModule,
  ]
})
export class AccountModule { }
