import { Component } from '@angular/core';
import { Category } from '../../models';
import { CategoryItemComponent } from './category-item/category-item.component';

@Component({
  selector: 'app-category-board',
  standalone: true,
  imports: [CategoryItemComponent],
  templateUrl: './category-board.component.html',
  styleUrl: './category-board.component.css'
})
export class CategoryBoardComponent {

  categories: Category[] = [
    { name: 'Oszczędności', current: 50, total: 500 },
    { name: 'Codzienne', current: 350, total: 800 },
    { name: 'Rozrywka', current: 2220, total: 300 }
  ];
}
