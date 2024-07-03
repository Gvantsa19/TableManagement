import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { User } from '../models/user';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../shared/services/user.service';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  public currentUser: Observable<User> | null = null;
  public sharedEmail;
  baseUrl = environment.apiUrl;
  returnUrl;

  constructor(
    private http: HttpClient,
    private router: Router,
    private jwtHelper: JwtHelperService,
    private route: ActivatedRoute,
    private userService: UserService
  ) {
    // Possible route fix
    // router.events
    //   .pipe(filter(event => event instanceof NavigationEnd))
    //   .subscribe((event: NavigationEnd) => {
    //     console.log('prev:', event.url);
    //     this.returnUrl = event.url;
    //   });
  }

  login(values: { username; password }) {
    return this.http.post<User>(this.baseUrl + 'Auth/login', values).pipe(
      map((user) => {
        this.userService.setCurrentUserToken(user);
        this.returnUrl =
          this.route.snapshot.queryParams['returnUrl'] || '/backoffice/tms';
        this.router.navigateByUrl(this.returnUrl).then((r) => r);
        // this.startThePresenceConnection(user.token);
        // this.getUnreadMessages(this.userService.id()).subscribe((res) => {
        //   localStorage.setItem('mm', res.toString());
        // });
        return user;
      })
    );
  }

  logout() {
    this.userService.removeToken();
    this.router.navigateByUrl('/');
    // if (this.presence.hubConnection) {
    //   this.presence.stopHubConnection();
    // }
  }

  isLoggedIn() {
    if (this.userService.token()) {
      // if (!this.presence.hubConnection) {
      //   this.presence.createHubConnection(this.userService.token());
      // }
      return true;
    } else {
      return false;
    }
  }

  public isAuthenticated(): boolean {
    return !this.jwtHelper.isTokenExpired(this.userService.token());
  }

  checkValidatePassword(query) {
    return this.http.get(
      this.baseUrl + 'user/validatepasswordrequest?query=' + query
    );
  }

  register(data) {
    return this.http.post<User>(this.baseUrl + 'Auth/register', data);
  }
}
