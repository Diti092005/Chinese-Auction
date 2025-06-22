import { Gift } from './gift.model';

export interface Donor {
  id: number;
  name: string;
  email: string;
  showMe?: boolean;
  gifts: Gift[];
}
