import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {Router} from 'aurelia-router';

@inject(Api, Router)
export class CreateCase {

    constructor(api, router) {
        this.api = api;
        this.router = router;
    }

    activate(params) {
        this.situation = params.situation;
    }

    createCase() {
        var page = this;
        
        this.api.cases.createCase(this.situation, this.title)
            .then(function(caseId) {
                if (!caseId) {
                    caseId = -1;
                }

                page.router.navigateToRoute('dashboard', { caseId: caseId });
            });
    }
}