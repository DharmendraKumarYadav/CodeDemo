
import { DealerListComponent } from './components/dealer-list/dealer-list.component';
import { UserListComponent } from './components/user-list/user-list.component';


import { Routes } from '@angular/router';


export const routes: Routes = [
  { path: 'users', component: UserListComponent},
  { path: 'dealer', component: DealerListComponent}
];
