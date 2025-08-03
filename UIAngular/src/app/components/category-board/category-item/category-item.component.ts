import { Component, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Transaction } from '../../../models';
import { CategoryEnum } from '../../../enums';

@Component({
  selector: 'app-category-item',
  standalone: true,
  templateUrl: './category-item.component.html',
  styleUrl: './category-item.component.css'
})
export class CategoryItemComponent {
  @Input() color!: string;
  @Input() transactions!: Transaction[];
  @Input() category!: string;
  progress = 0;
  totalValue: number = 0;
  currentValue: number = 0;
  constructor(private http: HttpClient) { }

  ngOnInit() {
     this.totalValue = this.transactions
                        .reduce((sum, transaction) => sum + transaction.price, 0);
     this.currentValue = this.transactions
                        .filter(t => t.isApproved)
                        .reduce((sum, transaction) => sum + transaction.price, 0);
    this.progress = (this.currentValue / this.totalValue) * 100;
  }


}
