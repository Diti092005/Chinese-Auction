import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';
import { User } from '../../services/user.service';
import { Router, RouterModule } from '@angular/router';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ButtonModule, InputTextModule, RouterModule],
  templateUrl: './sign-up-component.html',
  styleUrls: ['./sign-up-component.scss']
})
export class SignUpComponent {
  signUpForm: FormGroup;
  isSubmitting: boolean = false;
  message: string = '';
  isError: boolean = false;

  constructor(private fb: FormBuilder, private userservice: User, private router: Router) {
    this.signUpForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', Validators.required],
      phone: ['']
    });
  }
onSubmit() {
  if (this.signUpForm.valid) {
    this.isSubmitting = true;
    const { username, password, fullName, email, phone } = this.signUpForm.value;

    this.userservice.register(username, password, fullName, email, phone)
      .pipe(
        finalize(() => this.isSubmitting = false) // תמיד יוריד את הסנדינג בסיום
      )
      .subscribe({
        next: () => {
          this.isError = false;
          this.message = 'Registration successful!';
          this.signUpForm.reset();
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 1500);
        },
        error: (err) => {
          console.error('Registration failed', err);
          this.isError = true;

          if (err.status === 409) {
            this.message = 'Username already exists. Please choose another.';
          } else if (err.status === 400) {
            this.message = 'Invalid data. Please check your input.';
          } else {
            this.message = 'Registration failed. Please try again.';
          }
        }
      });

  } else {
    this.signUpForm.markAllAsTouched();
  }
}

  isInvalid(controlName: string): boolean {
    const control = this.signUpForm.get(controlName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onCancel() {
    this.router.navigate(['/login']);
  }
}
