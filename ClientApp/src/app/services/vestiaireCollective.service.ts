import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IVestiaireCollective } from '../models/vestiaireCollective';

@Injectable()
export class VestiaireService {

    constructor(private http: HttpClient) { }

    async getVestiaireProducts(selection: IVestiaireCollective): Promise<Observable<IVestiaireCollective[]>> {
        return await this.http.get<IVestiaireCollective[]>(`api/vestiairecollective?catalog=${selection.catalog}&brand=${selection.brand}&color=${selection.color}&condition=${selection.condition}&priceFrom=${selection.priceFrom}&priceTo=${selection.priceTo}&modele=${selection.modele}`)
            .pipe(catchError(this.handleError<IVestiaireCollective[]>('getVintedProducts', [])));
    }

    private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            console.error(error);
            return of(result as T);
        };
    }
}
