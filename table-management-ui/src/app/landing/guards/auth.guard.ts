import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { LoginService } from '../services/login.service';
import { UserService } from '../../shared/services/user.service';
import { AlertifyService } from '../../shared/services/alertify.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(
    public auth: LoginService,
    public router: Router,
    private alertify: AlertifyService,
    private userService: UserService
  ) {}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (!this.auth.isLoggedIn() || !this.auth.isAuthenticated()) {
      let message;
      if (!this.auth.isLoggedIn()) {
        message = 'You must log in.';
      } else {
        message = 'Your session has expired, please log in again.';
      }
      this.auth.logout();
      this.router.navigate(['/'], { queryParams: { returnUrl: state.url } });
      this.alertify.message(message);
      return false;
    }
    return true;
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    return this.canActivate(childRoute, state);
  }
}
