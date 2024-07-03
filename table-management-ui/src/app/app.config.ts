import {
  ApplicationConfig,
  importProvidersFrom,
  isDevMode,
  provideExperimentalZonelessChangeDetection,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {
  HTTP_INTERCEPTORS,
  HttpClientModule,
  provideHttpClient,
  withFetch,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { JWT_OPTIONS, JwtHelperService, JwtInterceptor, JwtModule } from '@auth0/angular-jwt';
import { MatIconModule } from '@angular/material/icon';
import { environment } from '../environments/environment';
// export function tokenGetter() {
//   return localStorage.getItem('currentUser');
// }
export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    JwtHelperService,
    JwtModule,
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
};
