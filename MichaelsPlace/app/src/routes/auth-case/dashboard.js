import {inject} from 'aurelia-framework';
import {activationStrategy} from 'aurelia-router';
import {Api} from 'services/api';

@inject(Api)

export class Dashboard {
    currentItem = [];

    constructor(api) {
        this.api = api;
    }

    activate(params) {
        this.case = params.caseId;
        return this.update();
    }

    update() {
        
    }

    select(item) {
        this.currentItem = item;
    }
}