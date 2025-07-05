import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TicketService } from '../../services/ticket.service';
import { Ticket } from '../../models/ticket.model';
import { Router } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { fork } from 'child_process';
import { log } from 'console';

@Component({
  selector: 'app-pending-tickets',
  standalone: true,
  imports: [
    CommonModule,
    CardModule,
    ButtonModule,
    FormsModule
  ],
  templateUrl: './pending-ticket.component.html',
  styleUrls: ['./pending-ticket.component.scss']
})
export class PendingTicketsComponent {
  private ticketService = inject(TicketService);
  private router = inject(Router);

  tickets: Ticket[] = [];
  selectedIds: number[] = [];

  ngOnInit() {
    this.ticketService.getPending().subscribe(tickets => {
      this.tickets = tickets;
    });
  }

  pendingTickets(): Ticket[] {
    return this.tickets;
  }

  isSelected(ticketId: number): boolean {
    return this.selectedIds.includes(ticketId);
  }
  selectAll() {
    this.selectedIds = this.tickets.map(t => t.id);
  }
  clearAll() {
    this.selectedIds = [];
  }

  toggleSelection(ticketId: number) {
    if (this.isSelected(ticketId)) {
      this.selectedIds = this.selectedIds.filter(id => id !== ticketId);
    } else {
      this.selectedIds.push(ticketId);
    }
  }

  selectedTicketIds(): number[] {
    return this.selectedIds;
  }

  getTotalSelectedPrice(): number {
    return this.tickets
      .filter(t => this.selectedIds.includes(t.id))
      .reduce((sum, t) => sum + t.gift.price, 0);
  }

  payTicket(ticketId: number) {
    const confirmPay = confirm("Are you sure you want to pay for this ticket?");
    if (!confirmPay) return;
    console.log(ticketId);
    this.router.navigate(['/credit-payment'], {
      state: { ticketIds: [ticketId] },
      queryParams: { ticketIds: ticketId }
    });
  }

  paySelected() {
    if (this.selectedIds.length === 0) return;

    const confirmPay = confirm("Are you sure you want to pay for the selected tickets?");
    if (!confirmPay) return;
    console.log(this.selectedIds);

    this.router.navigate(['/credit-payment'], {
      state: { ticketIds: [...this.selectedIds] },
      queryParams: { ticketIds: this.selectedIds.join(',') }
    });

  }

  deleteTicket(ticketId: number) {
    const confirmDelete = confirm("Are you sure you want to delete this ticket?");
    if (!confirmDelete) return;

    this.ticketService.delete(ticketId).subscribe(() => {
      this.tickets = this.tickets.filter(t => t.id !== ticketId);
      this.selectedIds = this.selectedIds.filter(id => id !== ticketId);
    });
  }
}
