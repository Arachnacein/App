import { Component, EventEmitter, Input, Output } from '@angular/core';
import { getMonthName } from '../../utils/dates-extensions';

@Component({
  selector: 'app-date-selector',
  standalone: true,
  templateUrl: './date-selector.component.html',
  styleUrl: './date-selector.component.css'
})
export class DateSelectorComponent {
  @Input() currentDate: Date = new Date();
  @Output() monthChange = new EventEmitter<'prev' | 'next'>();

  ngOnInit(): void {
    if (!this.currentDate)
      this.currentDate = new Date();
  }
  get monthLabel(): string {
    return getMonthName(this.currentDate);
  }
  prevMonth() {
    this.currentDate = new Date(this.currentDate.setMonth(this.currentDate.getMonth() - 1));
  }

  nextMonth() {
    this.currentDate = new Date(this.currentDate.setMonth(this.currentDate.getMonth() + 1));
  }
}
