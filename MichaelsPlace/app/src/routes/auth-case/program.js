import {inject} from 'aurelia-framework';
import {Api} from 'services/api';

@inject(Api)

export class Program {

    constructor(api) {
        this.api = api;
    }
}