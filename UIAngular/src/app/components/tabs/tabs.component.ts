import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-tabs',
  standalone: true,
  templateUrl: './tabs.component.html',
  styleUrl: './tabs.component.css'
})
export class TabsComponent {

  @Input() activeTab: string = 'WYDATKI';
  @Output() tabChange = new EventEmitter<string>();

  selectTab(tab: string) {
    this.tabChange.emit(tab);
  }
}
