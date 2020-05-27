import { IVinted } from './../models/vinted';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class VintedService {

    constructor(private http: HttpClient) { }

    async getVintedProducts(selection: IVinted): Promise<Observable<IVinted[]>> {
        return await this.http.get<IVinted[]>(`api/vinted/${selection.catalog}/${selection.brand}/${selection.color}/${selection.condition}/${selection.priceFrom}/${selection.priceTo}/${selection.modele}`)
            .pipe(catchError(this.handleError<IVinted[]>('getVintedProducts', [])));
    }

    private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            console.error(error);
            return of(result as T);
        };
    }
}
