import { Category } from "./category.model";
import { Donor } from "./donor.model";
import { Ticket } from "./ticket.model";
import { User } from "./user.model";


export interface Gift {
  id: number;
  giftName: string;
  donorId: number;
  donor: Donor;
  categoryId: number;
  category: Category;
  imageUrl?: string;
  details?: string;
  price: number;
  winnerId?: number;
  winner?: User;
  tickets?: Ticket[];
}
