import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { VintedListComponent } from './results/vinted-list.component';


export const appRoutes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'results', component: VintedListComponent },

];
