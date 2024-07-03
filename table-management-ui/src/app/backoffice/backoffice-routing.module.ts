import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from '../layout/dashboard/dashboard/dashboard.component';
import { AuthGuard } from '../landing/guards/auth.guard';
import { TableDetailComponent } from './tms/table/table-detail/table-detail.component';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    canActivate: [AuthGuard],
    data: {
      title: 'Dashboard - TableManagement',
    },
    children: [
      {
        path: '',
        redirectTo: 'tms',
        pathMatch: 'full',
      },
      {
        path: 'tms',
        canActivate: [AuthGuard],
        loadChildren: () => import('./tms/tms.module').then((m) => m.TmsModule),
        data: {
          title: 'Dashboard | TableManagement',
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BackofficeRoutingModule {}
