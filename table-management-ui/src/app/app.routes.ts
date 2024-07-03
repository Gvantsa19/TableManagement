import { Routes } from '@angular/router';
import { LoginComponent } from './landing/components/login/login/login.component';
import { LoginGuard } from './landing/guards/login.guard';
import { AuthGuard } from './landing/guards/auth.guard';
import { register } from 'module';
import { RegisterComponent } from './landing/components/register/register/register.component';

export const routes: Routes = [
  {
    path: '',
    component: LoginComponent,
    canActivate: [LoginGuard],
    data: {
      title: 'Login - TableManagement',
    },
  },
  {
    path: 'backoffice',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./backoffice/backoffice.module').then((m) => m.BackofficeModule),
  },
  {
    path: 'register',
    component: RegisterComponent,
    data: {
      title: 'Register - TableManagement',
    },
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full',
  },
];
