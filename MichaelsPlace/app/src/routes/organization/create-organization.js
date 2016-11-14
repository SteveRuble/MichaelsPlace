import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {Router} from 'aurelia-router';

@inject(Api, Router)
export class CreateOrganization {

    constructor(api, router) {
        this.api = api;
        this.router = router;
    }

    activate(params) {

    }

    createOrganization() {
        var page = this;
        
        this.api.organizations.createOrganization(this.title)
            .then(function(organizationId) {
                if (!organizationId) {
                    organizationId = -1;
                }

                page.router.navigateToRoute('organization-dashboard', { organizationId: organizationId });
            });
    }
}