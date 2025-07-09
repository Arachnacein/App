import { Component } from '@angular/core';
import { NavbarComponent } from './components/navbar/navbar.component';
import { TabsComponent } from './components/tabs/tabs.component';
import { ActionButtonsComponent } from './components/action-buttons/action-buttons.component';
import { DateSelectorComponent } from './components/date-selector/date-selector.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports:
    [NavbarComponent,
    TabsComponent,
      ActionButtonsComponent,
    DateSelectorComponent],
  standalone: true,
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'UIAngular';
}
