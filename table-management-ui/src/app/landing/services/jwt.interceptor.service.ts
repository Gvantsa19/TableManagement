import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { LoginService } from './login.service';
import { UserService } from '../../shared/services/user.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class JwtInterceptorService implements HttpInterceptor {
  baseUrl = environment.apiUrl;
  constructor(
    private loginService: LoginService,
    private userService: UserService
  ) {}
  
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // add auth header with jwt if user is logged in and request is to api url
    const isApiUrl = request.url.startsWith(this.baseUrl);
    if (this.loginService.isLoggedIn() && isApiUrl) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.userService.token()}`,
        },
      });
    }
    return next.handle(request);
  }
}
