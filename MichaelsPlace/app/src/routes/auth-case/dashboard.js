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
        this.caseId = params.caseId;
        return this.update();
    }

    update() {
        return this.api.cases.getCase(this.caseId)
            .then(myCase => this.caseObject = myCase);
    }

    select(item) {
        this.currentItem = item;
    }
}