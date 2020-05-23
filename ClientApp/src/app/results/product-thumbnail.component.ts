import { FormGroup } from '@angular/forms';
import { IVinted } from '../models/vinted';
import { Component, Input } from '@angular/core';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'product-thumbnail',
  template: `
    <div class="well hoverwell thumbnail">
      <h2>{{product?.brand | uppercase}}</h2>
      <p>Etat: {{form.controls.condition.value}}</p>
      <a href={{product?.link}}>Lien Vinted</a>
      <p>Couleur: {{form.controls.color.value}}</p>
      <img src={{product?.picture}} alt="{{product?.brand}}">
      <p>Prix: {{product?.price}} €</p>
    </div>
  `,
  styles: [`
    .thumbnail { min-height: 210px; }
    .pad-left { margin-left: 10px; }
    .well div { color: #bbb; }
  `]
})
export class ProductThumbnailComponent {
  @Input() product: IVinted;
  @Input() form: FormGroup;
}
