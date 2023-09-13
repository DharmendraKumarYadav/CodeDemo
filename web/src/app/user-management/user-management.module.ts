import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserComponent } from './components/user/user.component';
import { RouterModule } from '@angular/router';
import { routes } from './user-management.routes ';
import { SharedModule } from '../shared/shared.module';
import { AccountService } from '../shared/service/account.service';
import { AccountEndpoint } from '../shared/service/account-endpoint.service';
import { DealerListComponent } from './components/dealer-list/dealer-list.component';
import { DealerComponent } from './components/dealer-list/dealer/dealer.component';



@NgModule({
  declarations: [UserListComponent, UserComponent, DealerListComponent, DealerComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ],
  providers:[AccountService,AccountEndpoint ]
})
export class UserManagementModule { }
