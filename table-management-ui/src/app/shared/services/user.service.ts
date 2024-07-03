import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

const helper = new JwtHelperService();

@Injectable({
  providedIn: 'root',
})
export class UserService {
  userToken: any;

  constructor() {
    this.userToken = this.loadUserToken();
  }

  private loadUserToken() {
    // Check if localStorage is available (for browser environments)
    if (typeof localStorage !== 'undefined') {
      const storedUser = localStorage.getItem('currentUser');
      return storedUser ? JSON.parse(storedUser) : '';
    }
    return '';
  }

  id() {
    return parseInt(this.decodeToken().nameid, 10);
  }

  email() {
    return this.decodeToken().unique_name;
  }

  roles() {
    return this.decodeToken().roles.split(',');
  }

  readOnlyRoles() {
    // Return roles as array
    return this.decodeToken().readOnlyRoles.split(',');
  }
  isManager() {
    // Returns converted boolean
    return this.decodeToken().manager === 'true';
  }

  public token() {
    // Return token
    return this.userToken.token;
  }

  // Set token
  setCurrentUserToken(user) {
    this.userToken = user;
    localStorage.setItem('currentUser', JSON.stringify(user));
  }

  // Remove token
  removeToken() {
    this.userToken = '';
    localStorage.removeItem('currentUser');
  }

  // If role exists in current user roles
  isUserRole(role) {
    return this.roles().indexOf(role) !== -1;
  }

  // Does any role from provided exist in the current user roles
  isUserRoleAnyOf(roles) {
    const index = this.roles().findIndex(
      (element) => roles.indexOf(element) !== -1
    );
    return index !== -1;
  }

  // Get token and decode it
  private decodeToken() {
    return helper.decodeToken(this.userToken.token);
  }
}
