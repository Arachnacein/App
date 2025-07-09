import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-action-buttons',
  standalone: true,
  templateUrl: './action-buttons.component.html',
  styleUrl: './action-buttons.component.css'
})
export class ActionButtonsComponent {
  @Output() expense = new EventEmitter<void>();
  @Output() recurring = new EventEmitter<void>();
  @Output() income = new EventEmitter<void>();

  addExpense() {
    this.expense.emit();
  }
  addRecurring() {
    this.recurring.emit();
  }
  addIncome() {
    this.income.emit();
  }
}
