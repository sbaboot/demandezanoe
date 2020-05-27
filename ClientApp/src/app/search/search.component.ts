import { Component, OnInit, Output, Input, Inject } from '@angular/core';
import * as dataSitesJSON from '../../../../docs/dataSites.json';
import { IVinted } from '../models/vinted';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { VintedService } from '../services/vinted.service';
import { TOASTR_TOKEN, Toastr } from '../common/toastr.service';
import { convertResultsInId } from '../common/convertResultsInId';

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
    loading = false;

    constructor(private vintedService: VintedService, private formBuilder: FormBuilder, @Inject(TOASTR_TOKEN) private toastr: Toastr) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            catalog: [''],
            brand: [''],
            modele: [''],
            color: [''],
            condition: [''],
            priceFrom: [''],
            priceTo: ['']
        });
    }

    async onSubmit() {
        this.toastr.info('Recherche en cours', 'Patience');
        this.loading = true;
        this.isValidate = true;

        let selection = this.form.value;

        console.log('selection before', selection);
        selection = convertResultsInId('vinted', selection);
        console.log('converted selection', selection);

        (await this.vintedService.getVintedProducts(selection)).subscribe({
            next: (products: IVinted[]) => { this.products = products; },
            error: err => { this.toastr.error(err); },
            complete: () => { this.toastr.success('Liste récupérée', 'Vinted'); this.loading = false; }
        });
    }
}
