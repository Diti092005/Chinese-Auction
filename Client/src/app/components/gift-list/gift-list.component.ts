import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GiftService } from '../../services/gift.service';
import { Gift } from '../../models/gift.model';
import { Ticket } from '../../models/ticket.model';
import { FormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputNumberModule } from 'primeng/inputnumber';
import { MessageService } from 'primeng/api';
import { Auth } from '../../services/auth.service';
import { TicketService } from '../../services/ticket.service';
import { CategoryService } from '../../services/category.service';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-gift-list',
  standalone: true,
  imports: [CommonModule, FormsModule, CardModule, ButtonModule, InputNumberModule],
  providers: [MessageService],
  templateUrl: './gift-list.component.html',
  styleUrls: ['./gift-list.component.scss']
})
export class GiftListComponent implements OnInit {
  private giftService = inject(GiftService);
  private ticketService = inject(TicketService);
  private auth = inject(Auth);
  private categoryService = inject(CategoryService);
  private messageService = inject(MessageService);

  gifts: Gift[] = [];
  sortedGifts: Gift[] = [];
  categories: Category[] = [];
  primarySortField: 'price' | 'category' = 'price';
  primarySortDirection: 'asc' | 'desc' = 'asc';
  secondarySortField: 'price' | 'category' = 'category';
  secondarySortDirection: 'asc' | 'desc' = 'asc';
  selectedQuantities: { [giftId: number]: number } = {};
  // orderedTickets: Ticket[] = [];
  isSubmitting: boolean = false;

  ngOnInit() {
    this.loadCategoriesAndGifts();
  }

  loadCategoriesAndGifts() {
    this.categoryService.getAllCategories().subscribe(categories => {
      this.categories = categories;
      this.loadGifts();
    });
  }

  loadGifts() {
    this.giftService.getAllGifts().subscribe(gifts => {
      // מיפוי categoryName לפי categoryId במידה וחסר
      this.gifts = gifts.map(gift => {
        // הוספת קידומת ל-imageUrl אם צריך
        let imageUrl = gift.imageUrl;
        if (imageUrl && imageUrl.startsWith('/images')) {
          imageUrl = 'https://localhost:5001' + imageUrl;
        }
        if (!gift.categoryName) {
          const cat = this.categories.find(c => c.id === gift.categoryId);
          return { ...gift, categoryName: cat ? cat.name : gift.categoryId?.toString(), imageUrl };
        }
        return { ...gift, imageUrl };
      });
      this.sortGifts();
    });
  }

  sortGifts() {
    this.sortedGifts = [...this.gifts].sort((a, b) => {
      let compare = this.compareFields(a, b, this.primarySortField, this.primarySortDirection);
      if (compare === 0) {
        compare = this.compareFields(a, b, this.secondarySortField, this.secondarySortDirection);
      }
      return compare;
    });
  }

  compareFields(a: Gift, b: Gift, field: 'price' | 'category', direction: 'asc' | 'desc'): number {
    let compare = 0;
    if (field === 'price') {
      compare = a.price - b.price;
    } else if (field === 'category') {
      const catA = a.categoryName || '';
      const catB = b.categoryName || '';
      compare = catA.localeCompare(catB);
    }
    return direction === 'asc' ? compare : -compare;
  }

  onSortChange(field: 'price' | 'category', isPrimary: boolean) {
    if (isPrimary) {
      if (this.primarySortField === field) {
        this.primarySortDirection = this.primarySortDirection === 'asc' ? 'desc' : 'asc';
      } else {
        this.primarySortField = field;
        this.primarySortDirection = 'asc';
      }
      // למנוע ששני השדות יהיו אותו דבר
      if (this.secondarySortField === this.primarySortField) {
        this.secondarySortField = field === 'price' ? 'category' : 'price';
      }
    } else {
      if (this.secondarySortField === field) {
        this.secondarySortDirection = this.secondarySortDirection === 'asc' ? 'desc' : 'asc';
      } else {
        this.secondarySortField = field;
        this.secondarySortDirection = 'asc';
      }
      if (this.primarySortField === this.secondarySortField) {
        this.primarySortField = field === 'price' ? 'category' : 'price';
      }
    }
    this.sortGifts();
  }

  // loadPendingTickets() {
  //   this.ticketService.getPending().subscribe(tickets => {
  //     this.orderedTickets = tickets;
  //   });
  // }

  orderTickets(gift: Gift) {
    const quantity = this.selectedQuantities[gift.id] || 0;
    const userId = this.auth.getUserId();

    if (!userId) {
      alert('User not logged in.');
      return;
    }

    if (quantity <= 0) {
      alert('Please select a valid quantity.');
      return;
    }

    this.isSubmitting = true;

    const requests = Array.from({ length: quantity }).map(() => {
      const ticketData: Partial<Ticket> = {
        giftId: gift.id,
        userId: userId,
        status: 0
      };
      return this.ticketService.add(ticketData).toPromise();
    });

    Promise.all(requests)
      .then(() => {
        this.selectedQuantities[gift.id] = 0;
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Tickets ordered successfully!'
        });
      })
      .catch(() => alert('Failed to order some tickets.'))
      .finally(() => this.isSubmitting = false);
  }

  // removeTicket(ticketId: number) {
  //   this.ticketService.delete(ticketId).subscribe(() => {
  //     this.orderedTickets = this.orderedTickets.filter(t => t.id !== ticketId);
  //   });
  // }
}