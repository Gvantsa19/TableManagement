import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBarComponent } from '../../shared/components/mat-snack-bar/mat-snack-bar.component';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ErrorInterceptorService implements HttpInterceptor {
  constructor(private snackBar: MatSnackBarComponent) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      tap(
        (event) => {
          if (event instanceof HttpResponse) {
          }
        },
        (error) => {
          this.snackBar.openSnackBar(error.message, 'X');
          switch (error.status) {
            case 0:
              this.snackBar.openSnackBar(
                'API failed. Problems with Internet? (0)',
                'X',
                'blue-snackbar'
              );
              break;
            case 400:
              this.snackBar.openSnackBar(
                error.error || 'API Bad request (400)',
                'X'
              );
              break;
            case 401:
              this.snackBar.openSnackBar('Wrong credentials (401)', 'X');
              break;
            case 500:
              this.snackBar.openSnackBar('Server error (500)', 'X');
              break;
            case 404:
              this.snackBar.openSnackBar(
                `${error.error} (404)`,
                'X',
                'object-not-found-exception-snackbar'
              );
              break;
            case 422:
              this.snackBar.openSnackBar(
                `${error.error} (422)`,
                'X',
                'unprocessable-exception-snackbar'
              );
              break;
            default:
              this.snackBar.openSnackBar(`HTTP error ${error.status}`, 'X');
          }
        }
      )
    );
  }
}
