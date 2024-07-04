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
  containersArray!: FormArray;
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

  get updateColumnValues() {
    return this.updateForm.get('updateColumnValues') as FormArray;
  }

  logValidationErrors(group: FormGroup = this.tableForm): void {
    Object.keys(group.controls).forEach((key: string) => {
      const control = group.get(key);
      if (control instanceof FormGroup) {
        this.logValidationErrors(control);
      } else if (control instanceof FormArray) {
      } else {
        if (control && control.errors) {
          console.log(`Control ${key} has errors:`, control.errors);
        }
      }
    });
  }

  onSubmit(): void {
    // if (this.tableForm.invalid) {
    //   this.logValidationErrors();
    //   return;
    // }

    const formData = this.tableForm.value;
    const tableData = {
      tableName: formData.tableName,
      columns: formData.columns.map((column: any) => ({
        columnName: column.columnName,
        dataType: column.dataType,
        defaultValue:
          column.defaultValue === '' || column.dataType === 'serial'
            ? this.generateRandomValue(column.dataType)
            : column.defaultValue,
        isNullable: column.isNullable,
        isPrimaryKey: column.isPrimaryKey,
      })),
    };
    const update = {
      columnNames: formData.columns
        .filter((column: any) => column.columnName.toLowerCase() !== 'id')
        .map((column: any) => column.columnName),
        values: formData.columns
        .filter((column: any) => column.columnName.toLowerCase() !== 'id')
        .map((column: any) => this.generateRandomValue(column.dataType)),
    };
    this.tableService.createTable(tableData).subscribe(
      (response) => {
        this.tableCreated = true;
        setTimeout(() => {
          this.tableService
            .updateTable(this.tableForm.value.tableName, update)
            .subscribe(
              (updateResponse) => {
                console.log('Table updated successfully:', updateResponse);
                this.rout.navigate(['./backoffice']);
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
  generateRandomValue(dataType: string): any {
    switch (dataType) {
      case 'bigint':
        return Math.floor(Math.random() * Number.MAX_SAFE_INTEGER);
      case 'boolean':
        return Math.random() < 0.5;
      case 'char':
        return String.fromCharCode(65 + Math.floor(Math.random() * 26));
      case 'date':
        return new Date();
      case 'decimal':
        return Math.random() * 100;
      case 'double precision':
        return Math.random() * Number.MAX_VALUE;
      case 'integer':
        return Math.floor(Math.random() * Number.MAX_SAFE_INTEGER);
      case 'numeric':
        return Math.random() * 100;
      case 'real':
        return Math.random() * Number.MAX_VALUE;
      case 'smallint':
        return Math.floor(Math.random() * 32767);
      case 'text':
        return 'RandomText';
      case 'timestamp':
        return new Date().toISOString();
      case 'uuid':
        // Generate a UUID (for example purposes, use an actual UUID generation method)
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(
          /[xy]/g,
          function (c) {
            var r = (Math.random() * 16) | 0,
              v = c == 'x' ? r : (r & 0x3) | 0x8;
            return v.toString(16);
          }
        );
      case 'varchar':
        return 'RandomString';
      default:
        return null;
    }
  }
  addColumn(): void {
    this.columnsArray.push(this.createColumnFormGroup());
  }

  addContainer(): void {
    this.containersArray = this.tableForm.get('columns') as FormArray;
    this.containersArray.push(this.createColumnFormGroup());
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
