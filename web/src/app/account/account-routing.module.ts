import { ChnagePasswordComponent } from './component/chnage-password/chnage-password.component';
import { Routes, RouterModule } from '@angular/router';
import { PersonalDetailComponent } from './component/personal-detail/personal-detail.component';
import { NgModule } from '@angular/core';
import { AuthGuard } from '../shared/guards/auth.guard';

const routes: Routes = [
    {
      path: 'change-password',
      component: ChnagePasswordComponent,
      canActivate: [AuthGuard],
    },

    {
        path: 'profile',
        component: PersonalDetailComponent,
        canActivate: [AuthGuard],
    }
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class AccountRoutingModule { }