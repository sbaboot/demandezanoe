import { Component, OnInit, Output, Input } from '@angular/core';
import * as dataSitesJSON from '../../../../docs/dataSites.json';
import { IVinted } from '../models/vinted';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { VintedService } from '../services/vinted.service';


@Component({
    // tslint:disable-next-line: component-selector
    selector: 'search-component',
    templateUrl: './search.component.html'
})

export class SearchComponent implements OnInit {
    products: IVinted[];
    subscription: Subscription;
    form: FormGroup;
    isValidate = false;
    dataSitesJSON: any = (dataSitesJSON as any).default;

    constructor(private vintedService: VintedService, private formBuilder: FormBuilder) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            catalog: [''],
            brand: [''],
            modelBag: [''],
            color: [''],
            condition: [''],
            priceFrom: [''],
            priceTo: ['']
        });
    }

    async onSubmit() {
        const selection = this.form.value;
        (await this.vintedService.getVintedProducts(selection)).subscribe({
            next: (products: IVinted[]) => { this.products = products; },
            error: err => { console.log(err); },
            complete: () => console.log('Liste Vinted récupérée')
        }),
            this.isValidate = true;
    }
}
