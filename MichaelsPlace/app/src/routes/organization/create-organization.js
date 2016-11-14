import {inject, NewInstance} from 'aurelia-framework';
import {Api} from 'services/api';
import {Router} from 'aurelia-router';
import {ValidationController, ValidationRules} from 'aurelia-validation';

@inject(Api, Router, NewInstance.of(ValidationController))
export class CreateOrganization {

    constructor(api, router, controller) {
        this.api = api;
        this.router = router;
        this.controller = controller;
    }

    activate(params) {

    }

    createOrganization() {
        if (this.controller.validate().length <= 0) {
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
}

ValidationRules
    .ensure('name').required()
    .ensure('phoneNumber').required()
    .ensure('line1').required()
    .ensure('city').required()
    .ensure('state').required()
    .ensure('zip').required()
    .on(CreateOrganization);