import { CUSTOM_ELEMENTS_SCHEMA, Component, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { TableService } from '../../service/table.service';
import {
  MatDialog,
  MatDialogRef,
  MatDialogModule,
} from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-edit-table',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule, CommonModule],
  templateUrl: './add-edit-table.component.html',
  styleUrls: ['./add-edit-table.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AddEditTableComponent implements OnInit {
  tableForm!: FormGroup;
  updateForm!: FormGroup;
  tableCreated: boolean = false;
  tableData: any = { tableName: '', columns: [] };

  get columnsArray() {
    return this.tableForm.get('columns') as FormArray;
  }

  constructor(
    public dialogRef: MatDialogRef<AddEditTableComponent>,
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private tableService: TableService,
    private rout: Router
  ) {}

  ngOnInit(): void {
    this.tableForm = this.formBuilder.group({
      tableName: ['', Validators.required],
      columns: this.formBuilder.array([this.createColumnFormGroup()]),
    });

    this.updateForm = this.formBuilder.group({
      updateColumnValues: this.formBuilder.array([]),
    });
  }

  fetchTableData(table): void {
    // this.tableService.getById(table).subscribe((data) => {
    //   this.tableData = data;
    //   console.log(this.tableData.columns);
    //   this.tableData.columns.forEach((column: any) => {
    //     this.updateColumnValues.push(
    //       this.formBuilder.control('', Validators.required)
    //     );
    //   });
    // });
  }

  get updateColumnValues() {
    return this.updateForm.get('updateColumnValues') as FormArray;
  }

  onSubmit(): void {
    if (this.tableForm.valid) {
      const formData = this.tableForm.value;
      const tableData = {
        tableName: formData.tableName,
        columns: formData.columns.map((column: any) => ({
          columnName: column.columnName,
          dataType: column.dataType,
          defaultValue: column.defaultValue,
          isNullable: column.isNullable,
          isPrimaryKey: column.isPrimaryKey,
          tableId: 0,
        })),
      };

      const update = {
        columnNames: formData.columns.map((column: any) => column.columnName),
        values: formData.columns.map((column: any) => column.defaultValue),
      };
      this.tableService.createTable(tableData).subscribe(
        (response) => {
          this.tableCreated = true;
          this.fetchTableData(this.tableForm.value.tableName);

          setTimeout(() => {
            this.tableService
              .updateTable(this.tableForm.value.tableName, update)
              .subscribe(
                (updateResponse) => {
                  console.log('Table updated successfully:', updateResponse);
                  this.rout.navigate(["./backoffice"])
                },
                (updateError) => {
                  console.error('Error updating table:', updateError);
                }
              );
          }, 5000);
        },
        (error) => {
          console.error('Error creating table:', error);
        }
      );
    }
  }

  onUpdate(): void {
    if (this.updateForm.valid) {
      const columnNames = this.tableData.columns.map(
        (column: any) => column.columnName
      );
      const columnValues = this.updateForm.value.updateColumnValues;
      const update = { columnNames, columnValues };

      this.tableService.updateTable(this.tableData.tableName, update).subscribe(
        (updateResponse) => {
          console.log('Table updated successfully:', updateResponse);
          this.updateForm.reset();
        },
        (updateError) => {
          console.error('Error updating table:', updateError);
        }
      );
    }
  }

  addColumn(): void {
    this.columnsArray.push(this.createColumnFormGroup());
  }

  private createColumnFormGroup(): FormGroup {
    return this.formBuilder.group({
      columnName: ['', Validators.required],
      dataType: ['', Validators.required],
      defaultValue: ['', Validators.required],
      isNullable: [true],
      isPrimaryKey: [false],
    });
  }
}
