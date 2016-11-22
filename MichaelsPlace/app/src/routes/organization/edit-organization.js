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

    }

    EditOrganization() {
        this.controller.validate().then(errors => {
            if (errors.length === 0) {
                var page = this;
        
                this.api.organizations.editOrganization(
                        this.organizationId,
                        this.name,
                        this.phoneNumber,
                        this.faxNumber,
                        this.line1,
                        this.line2,
                        this.city,
                        this.state,
                        this.zip,
                        this.notes
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

