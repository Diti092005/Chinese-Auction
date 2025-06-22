import { Gift } from './gift.model';
import { User } from './user.model';
import { TicketStatus } from './ticket-status.model';

export interface Ticket {
  id: number;
  giftId: number;
  gift: Gift;
  userId: number;
  user: User;
  orderDate: string; // אפשר גם Date אבל JSON מגיע כ-string
  status: TicketStatus;
}
