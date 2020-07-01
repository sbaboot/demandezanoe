import { VestiaireService } from './services/vestiaireCollective.service';
import { VestiaireListComponent } from './results/vestiaire-collective-list.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InlineSVGModule } from 'ng-inline-svg';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { VintedListComponent, ProductThumbnailComponent } from './results/index';
import { SearchComponent } from './search/search.component';

import { TOASTR_TOKEN, Toastr } from './common/toastr.service';

import { registerLocaleData } from '@angular/common';
import localeFr from '@angular/common/locales/fr';
import { appRoutes } from './routes';
import { VintedService } from './services/vinted.service';
registerLocaleData(localeFr, 'fr');

const toastr: Toastr = window['toastr'];


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SearchComponent,
    VintedListComponent,
    VestiaireListComponent,
    ProductThumbnailComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    InlineSVGModule.forRoot(),
    RouterModule.forRoot(appRoutes)
  ],
  providers: [
    VintedService,
    VestiaireService,
    { provide: TOASTR_TOKEN, useValue: toastr },
    { provide: LOCALE_ID, useValue: 'fr-FR' },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
