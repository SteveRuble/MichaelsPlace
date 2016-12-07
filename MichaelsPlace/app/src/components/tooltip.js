import {customAttribute, inject, bindable} from 'aurelia-framework';
import $ from 'bootstrap';

@inject(Element)
@customAttribute('tooltip')
export class Tooltip {
    element: HTMLElement;
    @bindable title: any;
    @bindable placement: any;

    constructor(element) {
        this.element = element;
    }

    attached() {

        $(this.element).tooltip({
            title: this.title,
            placement: this.placement,
            container: 'body'
        });
    }
}