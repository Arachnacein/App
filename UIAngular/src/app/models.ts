import { CategoryEnum } from "./enums";

export interface Transaction {
  id: number;
  userId: string;
  name: string;
  description?: string;
  date: Date;
  price: number;
  category: CategoryEnum;
  isRecurring: boolean;
  isApproved: boolean;
}
