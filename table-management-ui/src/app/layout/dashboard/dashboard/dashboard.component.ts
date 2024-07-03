import { Component, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { UserService } from '../../../shared/services/user.service';
import { Router } from '@angular/router';
import { TableService } from '../../../backoffice/tms/table/service/table.service';
import { AddEditTableComponent } from '../../../backoffice/tms/table/add-edit-table/add-edit-table/add-edit-table.component';
import { MatDialog } from '@angular/material/dialog';
import { LoginService } from '../../../landing/services/login.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit, OnDestroy {
  public sidebarShow: boolean = true;
  private subs = new SubSink();

  tableData: any = {};
  filteredTableData: any[] = [];
  displayedColumns: string[] = ['columnName', 'dataType', 'isNullable'];
  pageSize: number = 10;
  PageNumber: number = 1;
  totalPages: number = 1;
  tableId;
  SortColumn: string = '';
  SortOrder: string = '';
  ColumnNameFilter: string = '';

  tableList;
  filteredTableNames;
  tableName;

  constructor(
    private userService: UserService,
    private loginService: LoginService,
    public dialog: MatDialog,
    private router: Router,
    private tableService: TableService
  ) {}

  ngOnInit(): void {
    this.getAllTable();
    this.fetchTableData(this.tableName);
  }

  getAllTable() {
    this.subs.sink = this.tableService.getAllTable().subscribe((res) => {
      this.tableList = res;
      this.filteredTableNames = this.tableList.filter(
        (name) => !name.startsWith('__EF') && !name.startsWith('AspNet')
      );
    });
  }

  openCreateTable() {
    const dialogRef = this.dialog.open(AddEditTableComponent, {
      data: { id: 0 },
      backdropClass: 'backdropBackground',
      minWidth: '100%',
      height: '100%',
    });
    this.subs.sink = dialogRef.afterClosed().subscribe(() => {
      this.getAllTable();
    });
  }

  logout() {
    this.loginService.logout();
  }

  fetchTableData(table): void {
    this.tableService.getById(table, this.PageNumber, this.pageSize).subscribe((data) => {
      this.tableData = data;
    });
  }

  setPage(page: number) {
    this.PageNumber = page;
    const startIndex = (this.PageNumber - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.filteredTableData = this.tableData.slice(startIndex, endIndex);
  }

  changeItemsPerPage(itemsPerPage: number): void {
    // this.pageSize = itemsPerPage;
    // this.PageNumber = 1;
    // this.fetchTableData(this.tableId);
  }

  sortData(column: string): void {
    // if (column === this.SortColumn) {
    //   this.SortOrder = this.SortOrder === 'asc' ? 'desc' : 'asc';
    // } else {
    //   this.SortColumn = column;
    //   this.SortOrder = 'asc';
    // }
    // this.fetchTableData(this.tableId);
  }

  applyFilter(event: Event) {
    this.ColumnNameFilter = (event.target as HTMLInputElement).value.toLowerCase();
    this.filteredTableData = this.tableData.items.filter((item) =>
      Object.values(item).some((val: any) =>
        String(val).toLowerCase().includes(this.ColumnNameFilter)
      )
    );
    this.totalPages = Math.ceil(this.filteredTableData.length / this.pageSize);
    const startIndex = (this.PageNumber - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.filteredTableData = this.filteredTableData.slice(startIndex, endIndex);
  }
  previousPage(): void {
    if (this.PageNumber > 1) {
      this.PageNumber--;
      this.fetchTableData(this.tableData.tableName);
    }
  }

  nextPage(): void {
    this.PageNumber++;
    this.fetchTableData(this.tableData.tableName);
  }
  ngOnDestroy(): void {}
}
