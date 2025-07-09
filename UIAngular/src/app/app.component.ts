import { Component } from '@angular/core';
import { NavbarComponent } from './components/navbar/navbar.component';
import { TabsComponent } from './components/tabs/tabs.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports: [NavbarComponent, TabsComponent],
  standalone: true,
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'UIAngular';
}
