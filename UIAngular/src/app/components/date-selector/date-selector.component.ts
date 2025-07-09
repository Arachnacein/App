import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-date-selector',
  standalone: true,
  templateUrl: './date-selector.component.html',
  styleUrl: './date-selector.component.css'
})
export class DateSelectorComponent {
  @Input() month: string = '';
  @Output() monthChange = new EventEmitter<'prev' | 'next'>();

  prevMonth() {
    this.monthChange.emit('prev');
  }

  nextMonth() {
    this.monthChange.emit('next');
  }
}
