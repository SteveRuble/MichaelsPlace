import {inject} from 'aurelia-framework';
import {Router, activationStrategy} from 'aurelia-router';
import {Api} from 'services/api';

@inject(Api, Router)

export class Dashboard {
    currentItem = [];

    constructor(api, router) {
        this.api = api;
        this.router = router;
    }

    activate(params) {
        this.caseId = params.caseId;
        return this.update();
    }
    update() {
        var dashboard = this;

        return this.api.cases.getCase(this.caseId)
            .then(function(myCase) {
                if (!myCase.id) {
                    dashboard.router.navigateToRoute('case-home');
                }

                dashboard.currentCase = myCase;
            });
    }
    select(item) {
        this.currentItem = item;
    }
}