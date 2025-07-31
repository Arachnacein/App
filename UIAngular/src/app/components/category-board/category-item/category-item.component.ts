import { Component, Input } from '@angular/core';
import { Category } from '../../../models';

@Component({
  selector: 'app-category-item',
  standalone: true,
  templateUrl: './category-item.component.html',
  styleUrl: './category-item.component.css'
})
export class CategoryItemComponent {

  @Input() category!: Category;
  progress = 0;

  tasks: string[] = ['bilard', 'biedronka', 'passat'];

  ngOnInit() {
    if (this.category.total > 0) {
      this.progress = (this.category.current / this.category.total) * 100;
    }
  }
}
