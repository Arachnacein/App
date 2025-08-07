import { Component, Input } from '@angular/core';
import { CategoryItemComponent } from './category-item/category-item.component';
import { Transaction } from '../../models';
import { HttpClient } from '@angular/common/http';
import { CategoryEnum } from '../../enums';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-category-board',
  standalone: true,
  imports: [CategoryItemComponent],
  templateUrl: './category-board.component.html',
  styleUrl: './category-board.component.css'
})
export class CategoryBoardComponent {
  @Input() categoryEnum = CategoryEnum;
  @Input() transactions!: Transaction[];
  savesTransactions: Transaction[] = [];
  feesTransactions: Transaction[] = [];
  entertainmentTransactions: Transaction[] = [];
  constructor(private http: HttpClient) { }



  ngOnInit() {
    this.loadTransactions();
  }

  loadTransactions() {
    //this.http.get<Transaction[]>(`http://localhost:8010/api/transaction?userId=E43C0E23-8C72-48D6-9B4A-DDDC33AC6BF1`)
    //         .subscribe(data => {
    //           this.transactions = data;
    //           this.savesTransactions = data.filter(x => x.category == CategoryEnum.Saves)
    //           this.feesTransactions = data.filter(x => x.category == CategoryEnum.Fees);
    //           this.entertainmentTransactions = data.filter(x => x.category == CategoryEnum.Entertainment);
    //         });
    this.savesTransactions = [
      {
        id: 1,
        userId: '11111111-1111-1111-1111-111111111111',
        name: 'Oszczędności Styczeń',
        description: 'Przelew na konto oszczędnościowe',
        date: new Date('2025-01-10'),
        price: 500,
        category: CategoryEnum.Saves,
        isRecurring: true,
        isApproved: true
      },
      {
        id: 2,
        userId: '11111111-1111-1111-1111-111111111111',
        name: 'Poduszka finansowa',
        date: new Date('2025-02-10'),
        price: 300,
        category: CategoryEnum.Saves,
        isRecurring: false,
        isApproved: true
      }
    ];

     this.feesTransactions = [
      {
        id: 3,
        userId: '11111111-1111-1111-1111-111111111111',
        name: 'Czynsz',
        description: 'Mieszkanie',
        date: new Date('2025-03-01'),
        price: 1200,
        category: CategoryEnum.Fees,
        isRecurring: true,
        isApproved: true
      },
      {
        id: 4,
        userId: '11111111-1111-1111-1111-111111111111',
        name: 'Internet',
        date: new Date('2025-03-05'),
        price: 80,
        category: CategoryEnum.Fees,
        isRecurring: true,
        isApproved: true
      }
    ];

     this.entertainmentTransactions = [
      {
        id: 5,
        userId: '11111111-1111-1111-1111-111111111111',
        name: 'Netflix',
        date: new Date('2025-03-10'),
        price: 55,
        category: CategoryEnum.Entertainment,
        isRecurring: true,
        isApproved: true
      },
      {
        id: 6,
        userId: '11111111-1111-1111-1111-111111111111',
        name: 'Kino',
        date: new Date('2025-03-15'),
        price: 45,
        category: CategoryEnum.Entertainment,
        isRecurring: false,
        isApproved: false
      }
    ];
  }
}
