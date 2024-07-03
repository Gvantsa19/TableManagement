import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-mat-snack-bar',
  standalone: true,
  imports: [],
  templateUrl: './mat-snack-bar.component.html',
  styleUrl: './mat-snack-bar.component.scss'
})
export class MatSnackBarComponent {
  constructor(public snackBar: MatSnackBar) { }

  // this function will open up snackbar on top right position with custom background color (defined in css)
  openSnackBar(message: string, action: string, bgClass: string = 'bg-danger') {

    this.snackBar.open(message, action, {
      duration: 5000,
      verticalPosition: 'top',
      horizontalPosition: 'end',
      panelClass: [bgClass, 'snackbar-danger'],
    });
  }
}
