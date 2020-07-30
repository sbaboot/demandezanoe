import { JoliClosetService } from './../services/joliCloset.service.';
import { IJoliCloset } from './../models/joliCloset';
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
    productsJoliCloset: IJoliCloset[];
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
        private joliClosetService: JoliClosetService,
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

        this.getVintedList();
        this.getJoliClosetList();
        this.getVestiaireList();
    }

    onClearForm() {
        this.form.reset();
    }

    searchVinted() {
        this.toastr.info('Recherche en cours', 'Patience');
        this.loading = true;
        this.isValidate = true;
        this.getVintedList();
    }

    searchJoliCloset() {
        this.toastr.info('Recherche en cours', 'Patience');
        this.loading = true;
        this.isValidate = true;
        this.getJoliClosetList();
    }

    searchVestiaire() {
        this.toastr.info('Recherche en cours', 'Patience');
        this.loading = true;
        this.isValidate = true;
        this.getVestiaireList();
    }

    async getVintedList() {
        let selectionVinted = this.form.value;
        selectionVinted = convertResultsInId('vinted', selectionVinted);
        (await this.vintedService.getVintedProducts(selectionVinted)).subscribe({
            next: (products: IVinted[]) => { this.productsVinted = products; },
            error: err => { this.toastr.error(err); console.log(err); },
            complete: () => { this.toastr.success('Liste récupérée', 'Vinted'); this.loading = false; }
        });
    }

    async getVestiaireList() {
        let selectionVestiaire = this.form.value;
        selectionVestiaire = convertResultsInId('vestiaireCollective', selectionVestiaire);
        (await this.vestiaireService.getVestiaireProducts(selectionVestiaire)).subscribe({
            next: (products: IVestiaireCollective[]) => { this.productsVestiaire = products; },
            error: err => { this.toastr.error(err); console.log(err); },
            complete: () => { this.toastr.success('Liste récupérée', 'Vestiaire Collective'); this.loading = false; }
        });
    }

    async getJoliClosetList() {
        let selectionJoliCloset = this.form.value;

        selectionJoliCloset = convertResultsInId('joliCloset', selectionJoliCloset);
        (await this.joliClosetService.getJoliClosetProducts(selectionJoliCloset)).subscribe({
            next: (products: IJoliCloset[]) => { this.productsJoliCloset = products; },
            error: err => { this.toastr.error(err); console.log(err); },
            complete: () => { this.toastr.success('Liste récupérée', 'Joli Closet'); this.loading = false; }
        });
    }
}
