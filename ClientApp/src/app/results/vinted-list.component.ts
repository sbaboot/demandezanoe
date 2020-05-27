import { FormGroup } from '@angular/forms';
import { Component, Input } from '@angular/core';
import { IVinted } from '../models/vinted';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'vinted-list',
  template: `
    <div>
      <h1>Résultats Vinted :</h1>
      <hr/>
      <div class="row">
        <div *ngFor="let product of products" class="col-md-5">
          <product-thumbnail [product]="product" [form]="form"></product-thumbnail>
        </div>
      </div>
    </div>
    `
})

export class VintedListComponent {
  @Input() products: IVinted[];
  @Input() form: FormGroup;
  constructor() { }
}

