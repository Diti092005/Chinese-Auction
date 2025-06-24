import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { InputTextModule } from 'primeng/inputtext';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { User } from '../../services/user';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    RouterModule,
    ReactiveFormsModule,
    DividerModule,
    ButtonModule,
    InputTextModule,
    ProgressSpinnerModule,
    CommonModule
  ],
  templateUrl: './login.html',
  styleUrls: ['./login.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  message: string = '';
  isError: boolean = false;
  loading: boolean = false;

  constructor(private fb: FormBuilder, private user: User,private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onLogin() {
    if (this.loginForm.invalid) {
      this.isError = true;
      this.message = 'Please fill in all required fields.';
      return;
    }

    this.loading = true;
    this.message = '';
    this.isError = false;

    const { username, password } = this.loginForm.value;

    this.user.login(username, password).subscribe({
      next: (response) => {
        console.log('Login success', response);
        this.isError = false;
        this.message = 'Successfully logged in!';
        this.loginForm.reset();
        this.loading = false;
      },
      error: (err) => {
        console.error('Login failed', err);
        this.isError = true;
        this.message = 'Incorrect username or password.';
        this.loading = false;
      }
    });

    this.router.navigate(['/home'])
  }
}