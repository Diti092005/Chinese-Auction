import { Component } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { InputMaskModule } from 'primeng/inputmask';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@Component({
  selector: 'app-credit-payment',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    InputMaskModule,
    ButtonModule,
    CardModule,
    ProgressSpinnerModule
  ],
  templateUrl: './credit-payment-component.html',
  styleUrls: ['./credit-payment-component.scss']
})
export class CreditPaymentComponent {
  paymentForm;
  focusedField: string | null = null;
  isProcessing = false;
  successMessage: string | null = null;

  constructor(private fb: FormBuilder) {
    this.paymentForm = this.fb.group({
      cardNumber: ['', [Validators.required]],
      cardHolder: ['', [Validators.required]],
      expiry: ['', [Validators.required, this.expiryValidator]],
      cvv: ['', [Validators.required]],
    });
  }

  expiryValidator(control: any) {
    const value = control.value;
    if (!value || value.length !== 5 || !/^\d{2}\/\d{2}$/.test(value)) {
      return { invalidExpiry: true };
    }
    const [month, year] = value.split('/').map(Number);
    if (month < 1 || month > 12) {
      return { invalidExpiry: true };
    }
    const now = new Date();
    const expiryDate = new Date(2000 + year, month);
    if (expiryDate < now) {
      return { expired: true };
    }
    return null;
  }

  submit() {
    if (this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched();
      this.successMessage = 'Please fill all fields correctly.';
      setTimeout(() => (this.successMessage = null), 3000);
      return;
    }

    this.isProcessing = true;

    setTimeout(() => {
      this.isProcessing = false;
      this.successMessage = 'Payment successful! 🎉';
      this.paymentForm.reset();
      this.focusedField = null;
      setTimeout(() => (this.successMessage = null), 4000);
    }, 2000);
  }

  onFocus(field: string) {
    this.focusedField = field;
  }

  onBlur() {
    this.focusedField = null;
  }
}
