import { AuthLayoutComponent } from './shared/components/auth-layout/auth-layout.component';
import { LayoutComponent } from './shared/components/layout/layout.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './shared/guards/auth.guard';


const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule),
    component: AuthLayoutComponent

  },

  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'dashboard'
      },
      {
        path: 'account',
        loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
      },
      {
        path: 'dashboard',
        loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule),
      },
      {
        path: 'user-management',
        loadChildren: () => import('./user-management/user-management.module').then(m => m.UserManagementModule),
      },
      {
        path: 'bike-spec-management',
        loadChildren: () => import('./bike-spec-management/bike-spec-management.module').then(m => m.BikeSpecManagementModule),
      },
      {
        path: 'location',
        loadChildren: () => import('./location-management/location-management.module').then(m => m.LocationManagementModule),
      },
      {
        path: 'showroom-management',
        loadChildren: () => import('./showroom-management/showroom-management.module').then(m => m.ShowroomManagementModule),
      }
      ,
      {
        path: 'bike-management',
        loadChildren: () => import('./bike-management/bike-management.module').then(m => m.BikeManagementModule),
      }
      ,
      {
        path: 'booking-management',
        loadChildren: () => import('./booking-management/booking-management.module').then(m => m.BookingManagementModule),
      }
      ,
      {
        path: 'general-management',
        loadChildren: () => import('./general-management/general-management.module').then(m => m.GeneralManagementModule),
      }
      ,
      {
        path: 'report-management',
        loadChildren: () => import('./report-management/report-management.module').then(m => m.ReportManagementModule),
      }
    ]

  },
  
  {
    path: 'error',
    component: AuthLayoutComponent,
    loadChildren: () => import('./error/error.module').then(m => m.ErrorModule),

  },
  {
    path: "**",
    redirectTo: "auth",
    pathMatch: "full"
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
