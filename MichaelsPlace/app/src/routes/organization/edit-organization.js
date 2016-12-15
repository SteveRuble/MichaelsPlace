import {inject, NewInstance} from 'aurelia-framework';
import {Api} from 'services/api';
import {Router} from 'aurelia-router';
import {ValidationController, ValidationRules} from 'aurelia-validation';

@inject(Api, Router, NewInstance.of(ValidationController))
export class EditOrganization {

    constructor(api, router, controller) {
        this.api = api;
        this.router = router;
        this.controller = controller;

        ValidationRules
            .ensure('name').required()
            .ensure('phoneNumber').required()
            .ensure('line1').required()
            .ensure('city').required()
            .ensure('state').required()
            .ensure('zip').required()
            .on(EditOrganization);
    }

    activate(params) {
        this.organizationId = params.organizationId;
        return this.update();
    }

    update() {
        var dashboard = this;

        return this.api.organizations.getOrganization(this.organizationId)
            .then(function(organization) {
                if (!organization) {
                    dashboard.router.navigateToRoute('organization-home');
                }

                dashboard.organization = organization;
            });
    }

    editOrganization() {
        this.controller.validate().then(errors => {
            if (errors.length === 0) {
                var page = this;
        
                this.api.organizations.editOrganization(
                        this.organization.name,
                        this.organization.phoneNumber,
                        this.organization.faxNumber,
                        this.organization.address.line1,
                        this.organization.address.line2,
                        this.organization.address.city,
                        this.organization.address.state,
                        this.organization.address.zip,
                        this.organization.notes,
                        this.organizationId
                    )
                    .then(function(organizationId) {
                        if (!organizationId) {
                            organizationId = -1;
                        }

                        page.router.navigateToRoute('organization-dashboard', { organizationId: organizationId });
                    });
            }
        });
    }
}

