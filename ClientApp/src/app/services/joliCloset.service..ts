import { IJoliCloset } from './../models/joliCloset';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class JoliClosetService {

    constructor(private http: HttpClient) { }

    async getJoliClosetProducts(selection: IJoliCloset): Promise<Observable<IJoliCloset[]>> {
        return await this.http.get<IJoliCloset[]>(`api/jolicloset?catalog=${selection.catalog}&brand=${selection.brand}&modele=${selection.modele}`)
            .pipe(catchError(this.handleError<IJoliCloset[]>('getVintedProducts', [])));
    }

    private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            console.error(error);
            return of(result as T);
        };
    }
}
