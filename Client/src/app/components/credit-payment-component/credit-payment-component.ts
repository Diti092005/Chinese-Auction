import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { InputTextModule } from 'primeng/inputtext';
import { InputMaskModule } from 'primeng/inputmask';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { TicketService } from '../../services/ticket.service';
import { Ticket } from '../../models/ticket.model';
import { forkJoin } from 'rxjs';

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
export class CreditPaymentComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private ticketService = inject(TicketService);
  private fb = inject(FormBuilder);

  paymentForm!: FormGroup;
  tickets: Ticket[] = [];
  ticketIdsForPayment: number[] = [];
  totalPrice = 0;
  returnUrl = '/';
  isProcessing = false;
  successMessage = '';
  focusedField: string | null = null;


  ngOnInit() {
    this.paymentForm = this.fb.group({
      cardNumber: ['', [Validators.required]],
      cardHolder: ['', [Validators.required]],
      expiry: ['', [Validators.required]],
      cvv: ['', [Validators.required]]
    });

    // Try to get ticketIds from state, then from query string
    let ids: number[] = [];
    const nav = this.router.getCurrentNavigation();
    if (nav?.extras.state && Array.isArray(nav.extras.state['ticketIds'])) {
      ids = [...nav.extras.state['ticketIds']];
    }
    // If not found in state, check query string
    if (ids.length === 0) {
      const query = this.route.snapshot.queryParamMap;
      if (query.get('ticketIds')) {
        ids = query.get('ticketIds')!.split(',').map(id => +id).filter(id => !isNaN(id));
      } else if (query.get('ticketId')) {
        const singleId = +query.get('ticketId')!;
        if (!isNaN(singleId)) ids = [singleId];
      }
      this.returnUrl = query.get('returnUrl') ?? '/purchase';
    } else {
      // If found in state, also check for returnUrl in query string
      const query = this.route.snapshot.queryParamMap;
      this.returnUrl = query.get('returnUrl') ?? '/purchase';
    }

    console.log('CreditPaymentComponent - ticketIds:', ids);

    if (ids.length > 0) {
      const requests = ids.map(id => this.ticketService.getById(id));
      forkJoin(requests).subscribe(results => {
        this.tickets = results.filter((t): t is Ticket => !!t);
        this.ticketIdsForPayment = this.tickets.map(t => t.id);
        this.totalPrice = this.tickets.reduce((sum, t) => sum + t.gift.price, 0);
      });
    } else {
      alert('No tickets selected for payment or the link is invalid.');
      this.router.navigateByUrl('/purchase');
      this.tickets = [];
      this.ticketIdsForPayment = [];
      this.totalPrice = 0;
    }
  }


  submit() {
    if (this.paymentForm.invalid || this.isProcessing) {
      this.paymentForm.markAllAsTouched();
      return;
    }

    this.isProcessing = true;

    this.ticketService.pay(this.ticketIdsForPayment).toPromise()
      .then(() => {
        this.successMessage = 'Payment completed successfully!';
        this.paymentForm.reset();
        this.isProcessing = false;
        setTimeout(() => {
          this.router.navigateByUrl(this.returnUrl);
        }, 2000);
      })
      .catch(() => {
        alert('Payment failed. Please try again.');
        this.isProcessing = false;
      });
  }

  onFocus(field: string) {
    this.focusedField = field;
  }

  onBlur() {
    this.focusedField = null;
  }
}
