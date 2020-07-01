import { IVestiaireCollective } from '../models/vestiaireCollective';
import { FormGroup } from '@angular/forms';
import { Component, Input } from '@angular/core';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'vestiaire-list',
    template: `
    <div>
        <hr/>
        <div class="row">
        <div *ngFor="let product of products" class="col-md-4">
            <product-thumbnail [product]="product" [form]="form"></product-thumbnail>
        </div>
        </div>
    </div>
    `
})

export class VestiaireListComponent {
    @Input() products: IVestiaireCollective[];
    @Input() form: FormGroup;
    constructor() { }
}

