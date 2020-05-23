import * as dataSitesJson from '../../../../docs/dataSites.json';

export interface IVinted {
    id: number;
    catalog: string;
    picture: string;
    link: string;
    brand: string;
    color: string;
    modelBag: string;
    condition: Conditions;
    priceFrom: string;
    priceTo: string;
}

enum Conditions {
    NewWithLabel = 6,
    New = 1,
    VeryGoodState = 2,
    GoodState = 3,
    Satisfactory = 4,
    Null = 0
}
