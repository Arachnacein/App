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
    //this.http.get<Transaction[]>(`${environment.apiUrl}/api/transaction?userId=E43C0E23-8C72-48D6-9B4A-DDDC33AC6BF1`)
    console.log("Before httpGet");
    this.http.get<Transaction[]>(`http://localhost:8010/api/transaction?userId=E43C0E23-8C72-48D6-9B4A-DDDC33AC6BF1`)
             .subscribe(data => {
               this.transactions = data;
               this.savesTransactions = data.filter(x => x.category == CategoryEnum.Saves)
               this.feesTransactions = data.filter(x => x.category == CategoryEnum.Fees);
               this.entertainmentTransactions = data.filter(x => x.category == CategoryEnum.Entertainment);
             });
    console.log(this.transactions);
  }
}
