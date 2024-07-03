import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TableDetailComponent } from './table/table-detail/table-detail.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'tms'
  },
  {
    path: 'table-detail/:tableId',
    component:TableDetailComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TmsRoutingModule { }
