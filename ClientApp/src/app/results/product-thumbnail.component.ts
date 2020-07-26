import { FormGroup } from '@angular/forms';
import { Component, Input } from '@angular/core';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'product-thumbnail',
  templateUrl: 'product-thumbnail.component.html',
  styles: [`
    .thumbnail { min-height: 210px; }
    .pad-left { margin-left: 10px; }
    .well div { color: #bbb; }
  `]
})
export class ProductThumbnailComponent {
  @Input() product: any;
  @Input() form: FormGroup;
}
