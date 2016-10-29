import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {Router} from 'aurelia-router';

@inject(Api, Router)
export class Index {

    constructor(api, router) {
        this.api = api;
        this.router = router;

        this.title = "";
    }

    activate(params) {
        this.situation = params.situation;
    }

    createCase() {
        var newCase = this.api.cases.createCase(this.situation, this.title)
            .then(caseId => this.caseId = caseId);
        if (this.caseId == null) {
            this.caseId = '2e8fa0daac1e491592b3e03e99be682f';
        }
        this.router.navigateToRoute('dashboard', {caseId : this.caseId});
    }
}