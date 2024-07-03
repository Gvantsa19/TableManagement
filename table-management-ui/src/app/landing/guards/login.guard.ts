import { CanActivateFn } from '@angular/router';

export const loginGuard: CanActivateFn = (route, state) => {
  return true;
};
import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { LoginService } from '../services/login.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../models/user';
import { UserService } from '../../shared/services/user.service';
import { AlertifyService } from '../../shared/services/alertify.service';

@Injectable({
  providedIn: 'root',
})
export class LoginGuard implements CanActivate {
  constructor(
    private loginService: LoginService,
    private router: Router,
    private jwtHelper: JwtHelperService,
    private alertify: AlertifyService,
    private userService: UserService
  ) {}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (!this.loginService.isLoggedIn()) {
      return true;
    } else {
      if (this.jwtHelper.isTokenExpired(this.userService.token())) {
        this.userService.removeToken();
        this.router.navigate(['/']);
        this.alertify.message('Your session has expired, please log in again.');
        return true;
      } else {
        this.router.navigate(['/backoffice']);
        return false;
      }
    }
  }
}
