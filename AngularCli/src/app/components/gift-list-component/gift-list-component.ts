import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TicketService } from '../../services/ticket-service';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-gift-list',
  templateUrl: './gift-list-component.html',
  standalone:true,
  imports:[CommonModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    ],
  styleUrls: ['./gift-list-component.scss']
})
export class GiftListComponent implements OnInit {
  gifts: any[] = [];
  quantities: { [giftId: number]: number } = {};
  paidTickets: any[] = [];

  constructor(
    private http: HttpClient,
    @Inject(TicketService) private ticketService: TicketService
  ) {}

  ngOnInit() {
    this.loadGifts();
    this.loadPaidTickets();
  }

  loadGifts() {
    this.http.get('https://localhost:5001/api/Gifts')
      .subscribe({
        next: (res: any) => this.gifts = res,
        error: (err) => console.error('Failed to load gifts', err)
      });
  }

  loadPaidTickets() {
    this.ticketService.getPaidTicketsByUser()
      .subscribe({
        next: (res: any) => this.paidTickets = res,
        error: (err: any) => console.error('Failed to load paid tickets', err)
      });
  }

  increaseQuantity(giftId: number) {
    this.quantities[giftId] = (this.quantities[giftId] || 0) + 1;
  }

  decreaseQuantity(giftId: number) {
    if (this.quantities[giftId] > 0) {
      this.quantities[giftId]--;
    }
  }

  getQuantity(giftId: number): number {
    return this.quantities[giftId] || 0;
  }

  getPaidCount(giftId: number): number {
    return this.paidTickets.filter(t => t.giftId === giftId).length;
  }

  getTotalAmount(): number {
    let total = 0;
    for (const giftId in this.quantities) {
      const gift = this.gifts.find(g => g.id == +giftId);
      if (gift) {
        total += this.quantities[giftId] * gift.price;
      }
    }
    return total;
  }

  pay() {
    const ticketRequests = [];

    for (const giftId in this.quantities) {
      const quantity = this.quantities[giftId];
      for (let i = 0; i < quantity; i++) {
        ticketRequests.push(
          this.ticketService.addTicket({ giftId }).toPromise()
        );
      }
    }

    Promise.all(ticketRequests)
      .then((results: any) => {
        const ticketIds = results.map((ticket: any) => ticket.id);
        return this.ticketService.payTickets(ticketIds).toPromise();
      })
      .then(() => {
        console.log('Payment completed successfully');
        this.loadPaidTickets();
        this.quantities = {};
      })
      .catch(err => console.error('Payment failed', err));
  }
}
