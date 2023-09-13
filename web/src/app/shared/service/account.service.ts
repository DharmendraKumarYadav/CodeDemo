import { UserEdit, UserRegister } from '../model/auth/user-edit.model';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { Injectable } from '@angular/core';
import { Observable, Subject, forkJoin } from 'rxjs';
import { mergeMap, tap } from 'rxjs/operators';
import { AccountEndpoint } from './account-endpoint.service';
import { Role } from 'src/app/shared/model/auth/role.model';
import { User } from 'src/app/shared/model/auth/user.model';


@Injectable()
export class AccountService  {

  constructor(
    private authService: AuthenticationService,
    private accountEndpoint: AccountEndpoint) {

  }

  updateCurrentUser(user: any) {
    return this.accountEndpoint.getUpdateUserEndpoint(user, null);
  }
  updatePassword(passwordObject: any) {
    return this.accountEndpoint.getPasswordChangeEndpoint(passwordObject);
  }



  getUser(userId?: string) {
    return this.accountEndpoint.getUserEndpoint<User>(userId);
  }
  getUsers(page?: number, pageSize?: number) {

    return this.accountEndpoint.getUsersEndpoint<User[]>(page, pageSize);
  }

  updateUser(user: UserEdit) {
    if (user.id) {
      return this.accountEndpoint.getUpdateUserEndpoint(user, user.id);
    } else {
      return this.accountEndpoint.getUserByUserNameEndpoint<User>(user.userName).pipe(
        mergeMap(foundUser => {
          user.id = foundUser.id;
          return this.accountEndpoint.getUpdateUserEndpoint(user, user.id);
        }));
    }
  }
  createUser(user: UserRegister) {
    return this.accountEndpoint.getNewUserEndpoint<User>(user);
  }



  deleteUser(userOrUserId: string | User): Observable<User> {
    if (typeof userOrUserId === 'string' || userOrUserId instanceof String) {
      return this.accountEndpoint.getDeleteUserEndpoint<User>(userOrUserId as string)
    } else {
      if (userOrUserId.id) {
        return this.deleteUser(userOrUserId.id);
      } else {
        return this.accountEndpoint.getUserByUserNameEndpoint<User>(userOrUserId.userName).pipe<User>(
          mergeMap(user => this.deleteUser(user.id)));
      }
    }
  }

  unblockUser(userId: string) {
    return this.accountEndpoint.getUnblockUserEndpoint(userId);
  }


  refreshLoggedInUser() {
    return this.accountEndpoint.refreshLogin();
  }

  getRoles(page?: number, pageSize?: number) {
    return this.accountEndpoint.getRolesEndpoint<Role[]>(page, pageSize);
  }
  getDealers() {
    return this.accountEndpoint.getDealerEndpoint<any>();
  }
  updateDealerOrder(formdata:any) {
    return this.accountEndpoint.updateDealerOrderEndpoint<any>(formdata.id,formdata.orderId);
  }

}
