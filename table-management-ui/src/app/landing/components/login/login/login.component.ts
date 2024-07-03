import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginService } from '../../../services/login.service';
import { MatDialog } from '@angular/material/dialog';
import { first } from 'rxjs';
import { SubSink } from 'subsink';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, HttpClientModule], 
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit, OnDestroy {
  loginForm!: FormGroup;
  reset = false;
  emailFocusOut!: boolean;
  hide = true;
  isBeingLoggedIn = false;
  private subs = new SubSink();

  constructor(private loginService: LoginService, public dialog: MatDialog, private router: Router) {
    this.initializeForm();
  }

  get f() {
    return this.loginForm.controls;
  }

  ngOnInit(): void {}

  initializeForm(): void {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', Validators.required),
    });
  }

  onSubmit() {
    this.isBeingLoggedIn = true;
    if (this.loginForm.invalid) {
      this.loginForm.disable();
    } else {
      this.subs.sink = this.loginService
        .login(this.loginForm.value)
        .pipe(first())
        .subscribe(
          (data) => {
            this.loginService.sharedEmail = this.f['email'].value;
          },
          (error) => {
            this.isBeingLoggedIn = false;
            this.loginForm.enable();
          }
        );
    }
  }

  redirectToRegister() {
    this.router.navigate(['/register']);
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
