import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { TabsComponent } from './components/tabs/tabs.component';
import { ActionButtonsComponent } from './components/action-buttons/action-buttons.component';
import { DateSelectorComponent } from './components/date-selector/date-selector.component';
import { CategoryBoardComponent } from './components/category-board/category-board.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AppComponent,
    NavbarComponent,
    TabsComponent,
    ActionButtonsComponent,
    DateSelectorComponent,
    CategoryBoardComponent,

    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
