import { IVestiaireCollective } from './../models/vestiaireCollective';
import { IVinted } from './../models/vinted';
import * as dataSitesJSON from '../../../../docs/dataSites.json';

export function convertResultsInId(site: string, selection: any) {
    let result = {};
    switch (site) {
        case 'vinted':
            result = {
                catalog: dataSitesJSON.vinted.catalogs[0].catalogs.find(c => c.title.toLowerCase() === selection.catalog.toLowerCase())
                    ? dataSitesJSON.vinted.catalogs[0].catalogs.find(c => c.title.toLowerCase() === selection.catalog.toLowerCase())
                        .id.toString()
                    : '0',
                brand: dataSitesJSON.vinted.brands.find(b => b.title.toLowerCase() === selection.brand.toLowerCase())
                    ? dataSitesJSON.vinted.brands.find(b => b.title.toLowerCase() === selection.brand.toLowerCase()).id.toString()
                    : '0',
                modele: selection.modele === ''
                    ? '0'
                    : hasWhiteSpace(selection.modele)
                        ? selection.modele.replace(' ', '%20')
                        : selection.modele,
                color: dataSitesJSON.vinted.colors.find(c => c.title.toLowerCase() === selection.color.toLowerCase())
                    ? dataSitesJSON.vinted.colors.find(c => c.title.toLowerCase() === selection.color.toLowerCase()).id.toString()
                    : '0',
                condition: dataSitesJSON.vinted.conditions.find(c => c.title.toLowerCase() === selection.condition.toLowerCase())
                    ? dataSitesJSON.vinted.conditions.find(c => c.title.toLowerCase() === selection.condition.toLowerCase()).id.toString()
                    : '0',
                priceFrom: selection.priceFrom !== '' ? selection.priceFrom.toString() : '0',
                priceTo: selection.priceTo !== '' ? selection.priceTo.toString() : '0'
            };
            return result;
        case 'vestiaireCollective':
            result = {
                catalog: dataSitesJSON.vestiaireCollective.catalogs.find(c => c.title.toLowerCase() === selection.catalog.toLowerCase())
                    ? dataSitesJSON.vestiaireCollective.catalogs.find(c => c.title.toLowerCase() === selection.catalog.toLowerCase())
                        .slug
                    : '0',
                brand: dataSitesJSON.vestiaireCollective.brands.find(b => b.title.toLowerCase() === selection.brand.toLowerCase())
                    ? dataSitesJSON.vestiaireCollective.brands.find(b => b.title.toLowerCase() === selection.brand.toLowerCase()).slug
                    : '0',
                modele: selection.modele === ''
                    ? '0'
                    : hasWhiteSpace(selection.modele)
                        ? selection.modele.replace(' ', '%20')
                        : selection.modele,
                color: dataSitesJSON.vestiaireCollective.colors.find(c => c.title.toLowerCase() === selection.color.toLowerCase())
                    ? dataSitesJSON.vestiaireCollective.colors.find(c => c.title.toLowerCase() === selection.color.toLowerCase()).slug
                    : '0',
                condition: dataSitesJSON.vestiaireCollective.conditions.find(c => c.title.toLowerCase() === selection.condition.toLowerCase())
                    ? dataSitesJSON.vestiaireCollective.conditions.find(c => c.title.toLowerCase() === selection.condition.toLowerCase()).slug
                    : '0',
                priceFrom: selection.priceFrom !== '' ? selection.priceFrom.toString() : '0',
                priceTo: selection.priceTo !== '' ? selection.priceTo.toString() : '0'
            };
            return result;
    }
}

function hasWhiteSpace(string: string) {
    return string.indexOf(' ') >= 0;
}
