import { IVestiaireCollective } from './../models/vestiaireCollective';
import { VestiaireService } from './../services/vestiaireCollective.service';
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
    productsVinted: IVinted[];
    productsVestiaire: IVestiaireCollective[];
    subscription: Subscription;
    form: FormGroup;
    isValidate = false;
    dataSitesJSON: any = (dataSitesJSON as any).default;
    loading = false;
    showVintedList = false;
    showVestiaireList = false;

    constructor(
        private vintedService: VintedService,
        private vestiaireService: VestiaireService,
        private formBuilder: FormBuilder,
        @Inject(TOASTR_TOKEN) private toastr: Toastr
    ) { }

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

        let selectionVestiaire = this.form.value;
        let selectionVinted = this.form.value;

        selectionVinted = convertResultsInId('vinted', selectionVinted);
        (await this.vintedService.getVintedProducts(selectionVinted)).subscribe({
            next: (products: IVinted[]) => { this.productsVinted = products; },
            error: err => { this.toastr.error(err); console.log(err); },
            complete: () => { this.toastr.success('Liste récupérée', 'Vinted'); this.loading = false; }
        });

        selectionVestiaire = convertResultsInId('vestiaireCollective', selectionVestiaire);
        (await this.vestiaireService.getVestiaireProducts(selectionVestiaire)).subscribe({
            next: (products: IVestiaireCollective[]) => { this.productsVestiaire = products; },
            error: err => { this.toastr.error(err); console.log(err); },
            complete: () => { this.toastr.success('Liste récupérée', 'Vestiaire Collective'); this.loading = false; }
        });
    }
}
