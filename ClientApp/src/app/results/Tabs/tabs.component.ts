import {
    Component,
    ContentChildren,
    QueryList,
    AfterContentInit,
} from '@angular/core';

import { TabComponent } from './tab.component';

@Component({
    selector: 'my-tabs',
    template: `
      <ul class="nav nav-tabs">
        <li *ngFor="let tab of tabs" (click)="selectTab(tab)" [class.active]="!tab.active">
          <a class="text-primary">{{tab.title}}</a>
        </li>
      </ul>
      <ng-content></ng-content>
    `,
    styles: [
        `
    .active {
        text-align: right;
        cursor: pointer;
        margin: 10px;
        color:red;
    }
      `
    ]
})
export class TabsComponent implements AfterContentInit {

    @ContentChildren(TabComponent) tabs: QueryList<TabComponent>;

    // contentChildren are set
    ngAfterContentInit() {
        // get all active tabs
        const activeTabs = this.tabs.filter((tab) => tab.active);

        // if there is no active tab set, activate the first
        if (activeTabs.length === 0) {
            this.selectTab(this.tabs.first);
        }
    }

    selectTab(tabulation: any) {
        // deactivate all tabs
        this.tabs.toArray().forEach(tab => {
            return tab.active = false;
        });

        // activate the tab the user has clicked on.
        tabulation.active = true;
    }

}
