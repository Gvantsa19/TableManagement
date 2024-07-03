import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { TableService } from '../service/table.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-table-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './table-detail.component.html',
  styleUrl: './table-detail.component.scss',
})
export class TableDetailComponent implements OnInit, OnDestroy {
  tableData: any[] = [];
  filteredTableData: any[] = [];
  displayedColumns: string[] = ['columnName', 'dataType', 'isNullable'];
  pageSize: number = 10;
  currentPage: number = 1;
  totalPages: number = 1;
  tableId;
  tableName;
  sortColumn;
  sortDirection;
  filterTerm;
  private subs = new SubSink();

  constructor(
    private tableService: TableService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.subs.sink = this.route.paramMap.subscribe((params) => {
      this.tableName = params.get('name');
      if (this.tableName) {
        this.fetchTableData(this.tableName);
      }
    });
  }

  fetchTableData(id): void {
    // this.tableService
    //   .getById(
    //     id,
    //   )
    //   .subscribe((data) => {
    //     this.tableData = data;
    //   });
  }

  setPage(page: number) {
    this.currentPage = page;
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.filteredTableData = this.tableData.slice(startIndex, endIndex);
  }

  changeItemsPerPage(itemsPerPage: number): void {
    this.pageSize = itemsPerPage;
    this.currentPage = 1;
    this.fetchTableData(this.tableId);
  }

  sortData(column: string): void {
    if (column === this.sortColumn) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }
    this.fetchTableData(this.tableId);
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value.toLowerCase();
    this.filteredTableData = this.tableData.filter((item) =>
      item.columnName.toLowerCase().includes(filterValue)
    );
    this.totalPages = Math.ceil(this.filteredTableData.length / this.pageSize);
    this.setPage(1);
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
