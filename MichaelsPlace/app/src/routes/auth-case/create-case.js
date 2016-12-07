import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {Router} from 'aurelia-router';

@inject(Api, Router)
export class CreateCase {

    constructor(api, router, validationController) {
        this.api = api;
        this.router = router;
        this.validationController = validationController;
    }

    activate(params) {
        this.situation = params.situation;
    }

    /**
     * Creates a case with the specified information and redirects to the case dashboard.
     */
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