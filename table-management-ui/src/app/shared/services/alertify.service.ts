import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { ShareValueService } from './share-value.service';

@Injectable({
  providedIn: 'root',
})
export class AlertifyService {
  constructor(
    private snackBar: MatSnackBar,
    private router: Router,
    private sh: ShareValueService
  ) {}

  success(message: string) {
    this.snackBar.open(message, 'X', {
      duration: 4000,
      verticalPosition: 'top',
      horizontalPosition: 'end',
      panelClass: ['bg-success', 'text-light', 'snackbar-success'],
    });
  }

  message(message: string) {
    this.snackBar.open(message, 'X', {
      duration: 4000,
      verticalPosition: 'top',
      horizontalPosition: 'end',
      panelClass: ['bg-light', 'text-dark', 'snackbar-success'],
    });
  }

  newMessage(message: string, username) {
    const snack = this.snackBar.open(message, 'View', {
      duration: 8000,
      verticalPosition: 'bottom',
      horizontalPosition: 'end',
      panelClass: ['bg-dark', 'text-light'],
    });

    const audio = new Audio();
    audio.src = './assets/audio/notification03.mp3';
    audio.load();
    audio.play();

    snack.onAction().subscribe(() => {
      this.router.navigate(['/backoffice/chat']);
      this.sh.changeMessageDep(username);
    });
  }

  warning(message: string) {
    this.snackBar.open(message, 'X', {
      duration: 4000,
      verticalPosition: 'top',
      horizontalPosition: 'end',
      panelClass: ['bg-danger', 'text-light', 'snackbar-warning'],
    });
  }
}
