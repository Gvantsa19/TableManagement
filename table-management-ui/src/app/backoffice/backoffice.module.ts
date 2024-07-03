import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {MAT_FORM_FIELD_DEFAULT_OPTIONS} from '@angular/material/form-field';
import { BackofficeRoutingModule } from './backoffice-routing.module';
import { JwtInterceptor, JwtModule } from '@auth0/angular-jwt';
import { environment } from '../../environments/environment';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatSnackBarComponent } from '../shared/components/mat-snack-bar/mat-snack-bar.component';
import { Title } from '@angular/platform-browser';
import { ErrorInterceptorService } from '../landing/services/error.interceptor.service';
import { MatIconModule } from '@angular/material/icon';
// export function tokenGetter() {
//   return localStorage.getItem('currentUser');
// }

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BackofficeRoutingModule,
    MatIconModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptorService,
      multi: true,
      deps: [MatSnackBarComponent],
    },
    {
      provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue: {appearance: 'fill'},
    },
    Title,
    MatSnackBarComponent,
  ],
})
export class BackofficeModule { }
