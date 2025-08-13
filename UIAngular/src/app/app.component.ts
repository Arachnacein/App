import { Component } from '@angular/core';
import { NavbarComponent } from './components/navbar/navbar.component';
import { TabsComponent } from './components/tabs/tabs.component';
import { ActionButtonsComponent } from './components/action-buttons/action-buttons.component';
import { CategoryBoardComponent } from './components/category-board/category-board.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports:
    [NavbarComponent,
      TabsComponent,
      ActionButtonsComponent,
      CategoryBoardComponent],
  standalone: true,
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Aplikacja budżetowa';
  currentDate: Date = new Date();

  onDateChange(newDate: Date) {
    this.currentDate = newDate;
  }
}
