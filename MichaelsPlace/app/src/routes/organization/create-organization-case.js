import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {Router} from 'aurelia-router';

@inject(Api, Router)
export class CreateOrganizationCase {

    constructor(api, router, validationController) {
        this.api = api;
        this.router = router;
        this.validationController = validationController;
    }

    activate(params) {
        this.situation = params.situation;
        this.organizationId = params.organizationId;
    }

    createCase() {
        var page = this;
        
        this.api.cases.createOrganizationCase(this.situation, this.title, this.organizationId)
            .then(function(caseId) {
                if (!caseId) {
                    caseId = -1;
                }

                page.router.navigateToRoute('organization-dashboard', { organizationId: page.organizationId });
            });
    }
}