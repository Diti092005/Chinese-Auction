import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GiftService } from '../../services/gift.service';
import { Gift } from '../../models/gift.model';
import { RouterModule, Router } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { DialogModule } from 'primeng/dialog';
import { FormsModule } from '@angular/forms';
import { Ticket } from '../../models/ticket.model';
import { CardModule } from 'primeng/card';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { TicketService } from '../../services/ticket.service';
import { TooltipModule } from 'primeng/tooltip';

@Component({
  selector: 'app-gifts',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    RouterModule,
    ConfirmDialogModule,
    DialogModule,
    FormsModule,
    CardModule,
    ToastModule,
    TooltipModule
  ],
  providers: [ConfirmationService, MessageService],
  templateUrl: 'gifts-manager.html',
  styleUrls: ['gifts-manager.scss']
})
export class GiftManagerComponent implements OnInit {
  gifts: Gift[] = [];
  allGifts: Gift[] = [];
  editDialogVisible = false;
  selectedGift: Gift | null = null;
  selectedGiftTickets: Ticket[] = [];
  ticketsDialogVisible = false;

  giftNameFilter = '';
  donorFilter = '';
  categoryFilter = '';

  constructor(
    private giftService: GiftService,
    private ticketService: TicketService,
    private router: Router,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) {}

  ngOnInit() {
    this.giftService.getAllGifts().subscribe((data: Gift[]) => {
      this.allGifts = data.filter((g: Gift) => g?.id);
      this.gifts = [...this.allGifts];
    });
  }

  onAddGift() {
    this.router.navigate(['/gifts/add']);
  }

  onEditGift(gift: Gift) {
    this.selectedGift = { ...gift };
    this.editDialogVisible = true;
  }

  onSaveEdit() {
    if (!this.selectedGift?.id) return;

    this.giftService.updateGift(this.selectedGift.id, this.selectedGift).subscribe({
      next: () => {
        this.reloadGifts();
        this.editDialogVisible = false;
      },
      error: err => {
        console.error('Failed to update gift', err);
      }
    });
  }
onDeleteGift(id: number) {
  console.log('Attempting to delete gift with ID:', id);
  this.ticketService.getByGiftId(id).subscribe({
    next: tickets => {
      if (tickets && tickets.length > 0) {
        this.messageService.add({
          severity: 'warn',
          summary: 'Deletion Not Allowed',
          detail: 'This gift cannot be deleted because it has associated tickets.'
        });
        return; // לא מאפשר המשך למחיקה
      }

      // אין כרטיסים – ממשיכים לשלב האישור
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete this gift?',
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          this.giftService.deleteGift(id).subscribe({
            next: () => this.reloadGifts(),
            error: err => {
              this.messageService.add({
                severity: 'error',
                summary: 'Server Error',
                detail: 'An unexpected error occurred while deleting the gift.'
              });
            }
          });
        }
      });
    },
    error: err => {
      console.error('Failed to check tickets for gift', err);
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Could not verify if the gift has tickets.'
      });
    }
  });
}







  private reloadGifts(): void {
    this.giftService.getAllGifts().subscribe((data: Gift[]) => {
      this.gifts = data.filter(g => g?.id);
    });
  }

  openTicketsDialog(gift: Gift): void {
    this.selectedGiftTickets = [];

    this.ticketService.getByGiftId(gift.id).subscribe({
      next: (tickets: Ticket[]) => {
        this.selectedGiftTickets = tickets;
        this.ticketsDialogVisible = true;
      },
      error: err => {
        console.error('Failed to load tickets for this gift', err);
        this.selectedGiftTickets = [];
        this.ticketsDialogVisible = true;
      }
    });
  }

  applyFilters() {
    this.gifts = this.allGifts.filter(gift =>
      (this.giftNameFilter ? gift.giftName?.toLowerCase().includes(this.giftNameFilter.toLowerCase()) : true) &&
      (this.donorFilter ? gift.donor?.name?.toLowerCase().includes(this.donorFilter.toLowerCase()) : true) &&
      (this.categoryFilter ? gift.category?.name?.toLowerCase().includes(this.categoryFilter.toLowerCase()) : true)
    );
  }
}
