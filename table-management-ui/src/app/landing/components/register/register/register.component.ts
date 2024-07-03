import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { first } from 'rxjs';
import { SubSink } from 'subsink';
import { LoginService } from '../../../services/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit, OnDestroy {
  registerForm!: FormGroup;
  reset = false;
  emailFocusOut!: boolean;
  hide = true;
  isBeingLoggedIn = false;
  private subs = new SubSink();

  constructor(
    private loginService: LoginService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.initializeForm();
  }

  get f() {
    return this.registerForm.controls;
  }

  ngOnInit(): void {}

  initializeForm(): void {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit() {
    if (this.registerForm.invalid) {
      this.registerForm.disable();
    } else {
      this.subs.sink = this.loginService
        .register(this.registerForm.value)
        .pipe(first())
        .subscribe(
          (data) => {
            this.router.navigate(['/'])
          },
          (error) => {}
        );
    }
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
