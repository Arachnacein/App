import { bootstrapApplication, platformBrowser } from '@angular/platform-browser';
import { registerLocaleData } from '@angular/common';
import localePl from '@angular/common/locales/pl';
import { AppComponent } from './app/app.component';
import { LOCALE_ID } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

registerLocaleData(localePl);


bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(),
    provideAnimations(),
    { provide: LOCALE_ID, useValue: 'pl' }
  ]
}).catch(err => console.error(err));


