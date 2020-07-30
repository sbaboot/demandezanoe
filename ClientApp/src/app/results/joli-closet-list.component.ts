import { IJoliCloset } from './../models/joliCloset';
import { FormGroup } from '@angular/forms';
import { Component, Input } from '@angular/core';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'jolicloset-list',
    template: `
    <div>
      <div class="row">
        <div *ngFor="let product of products" class="col-md-4">
          <product-thumbnail [product]="product" [form]="form"></product-thumbnail>
        </div>
      </div>
    </div>
    `
})

export class JoliClosetListComponent {
    @Input() products: IJoliCloset[];
    @Input() form: FormGroup;
    constructor() { }
}

