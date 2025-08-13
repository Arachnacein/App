import { Component, Input, SimpleChanges } from '@angular/core';
import { CategoryItemComponent } from './category-item/category-item.component';
import { Transaction } from '../../models';
import { HttpClient } from '@angular/common/http';
import { CategoryEnum } from '../../enums';
import { getMonthName } from '../../utils/dates-extensions';

@Component({
  selector: 'app-category-board',
  standalone: true,
  imports: [CategoryItemComponent],
  templateUrl: './category-board.component.html',
  styleUrl: './category-board.component.css'
})
export class CategoryBoardComponent {
  @Input() categoryEnum = CategoryEnum;
  @Input() selectedDate!: Date;
  transactions: Transaction[] = [];
  savesTransactions: Transaction[] = [];
  feesTransactions: Transaction[] = [];
  entertainmentTransactions: Transaction[] = [];
  currentDate: Date = new Date();
  constructor(private http: HttpClient) { }



  ngOnInit() {
    this.loadTransactions();
    this.sliceTransactionsByCategoryAndDate(this.selectedDate);

  }
  ngOnchanges(changes: SimpleChanges): void {
    if (changes['selectedDate']) 
      this.sliceTransactionsByCategoryAndDate(this.selectedDate);
  }

  loadTransactions() {
    this.http.get<Transaction[]>(`http://localhost:8010/api/transaction?userId=E43C0E23-8C72-48D6-9B4A-DDDC33AC6BF1`)
             .subscribe(data => {
               this.transactions = data.map(t => ({
                 ...t,
                 date: new Date(t.date)
               }));
               console.log('Transactions loaded:', this.transactions);
             });
    //this.savesTransactions = [
    //  {
    //    id: 1,
    //    userId: '11111111-1111-1111-1111-111111111111',
    //    name: 'Oszczędności Styczeń',
    //    description: 'Przelew na konto oszczędnościowe',
    //    date: new Date('2025-01-10'),
    //    price: 500,
    //    category: CategoryEnum.Saves,
    //    isRecurring: true,
    //    isApproved: true
    //  },
    //  {
    //    id: 2,
    //    userId: '11111111-1111-1111-1111-111111111111',
    //    name: 'Poduszka finansowa',
    //    date: new Date('2025-02-10'),
    //    price: 300,
    //    category: CategoryEnum.Saves,
    //    isRecurring: false,
    //    isApproved: true
    //  }
    //];

    // this.feesTransactions = [
    //  {
    //    id: 3,
    //    userId: '11111111-1111-1111-1111-111111111111',
    //    name: 'Czynsz',
    //    description: 'Mieszkanie',
    //    date: new Date('2025-03-01'),
    //    price: 1200,
    //    category: CategoryEnum.Fees,
    //    isRecurring: true,
    //    isApproved: true
    //  },
    //  {
    //    id: 4,
    //    userId: '11111111-1111-1111-1111-111111111111',
    //    name: 'Internet',
    //    date: new Date('2025-03-05'),
    //    price: 80,
    //    category: CategoryEnum.Fees,
    //    isRecurring: true,
    //    isApproved: true
    //  }
    //];

    // this.entertainmentTransactions = [
    //  {
    //    id: 5,
    //    userId: '11111111-1111-1111-1111-111111111111',
    //    name: 'Netflix',
    //    date: new Date('2025-03-10'),
    //    price: 55,
    //    category: CategoryEnum.Entertainment,
    //    isRecurring: true,
    //    isApproved: true
    //  },
    //  {
    //    id: 6,
    //    userId: '11111111-1111-1111-1111-111111111111',
    //    name: 'Kino',
    //    date: new Date('2025-03-15'),
    //    price: 45,
    //    category: CategoryEnum.Entertainment,
    //    isRecurring: false,
    //    isApproved: false
    //  }
    //];
  }
  sliceTransactionsByCategoryAndDate(date: Date) {
    this.savesTransactions = this.transactions.filter(x => x.category == CategoryEnum.Saves &&
                                                           x.date.getMonth() == date.getMonth() &&
                                                           x.date.getFullYear() == date.getFullYear());
    this.feesTransactions = this.transactions.filter(x => x.category == CategoryEnum.Fees &&
                                                          x.date.getMonth() == date.getMonth() &&
                                                          x.date.getFullYear() == date.getFullYear());
    this.entertainmentTransactions = this.transactions.filter(x => x.category == CategoryEnum.Entertainment &&
                                                                   x.date.getMonth() == date.getMonth() &&
                                                                   x.date.getFullYear() == date.getFullYear());
  }
  get monthLabel(): string {
    return getMonthName(this.currentDate);
  }
  prevMonth() {
    this.currentDate = new Date(this.currentDate.setMonth(this.currentDate.getMonth() - 1));
    this.sliceTransactionsByCategoryAndDate(this.currentDate);
  }

  nextMonth() {
    this.currentDate = new Date(this.currentDate.setMonth(this.currentDate.getMonth() + 1));
    this.sliceTransactionsByCategoryAndDate(this.currentDate);
  }
}
