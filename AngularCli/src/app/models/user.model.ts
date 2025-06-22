import { Ticket } from './ticket.model';
import { UserRole } from './userRole.model';

export interface User {
  id: number;
  username: string;
  fullName: string;
  email: string;
  password: string;
  phone?: string;
  role: UserRole;
  tickets: Ticket[];
}
