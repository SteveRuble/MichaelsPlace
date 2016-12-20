import {inject, NewInstance} from 'aurelia-framework';
import {Api} from 'services/api';
import {log} from 'services/log';
import {Router} from 'aurelia-router';
import {ValidationController, ValidationRules} from 'aurelia-validation';

@inject(Api, Router, NewInstance.of(ValidationController))
export class CreateOrganization {

    constructor(api, router, validationController) {
        this.api = api;
        this.router = router;
        this.validationController = validationController;

        ValidationRules
            .ensure('name').required()
            .ensure('phoneNumber').required()
            .ensure('line1').required().maxLength(200)
            .ensure('line2').maxLength(200)
            .ensure('city').required().maxLength(100)
            .ensure('state').required().maxLength(2)
            .ensure('zip').required().maxLength(12)
            .on(CreateOrganization);
    }

    createOrganization() {
        this.validationController.validate().then(errors => {
            if (errors.length === 0) {
                var page = this;
        
                this.api.organizations.createOrganization(
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
                            log.error('Unable to create organization.');
                            organizationId = -1;
                        }

                        page.router.navigateToRoute('organization-dashboard', { organizationId: organizationId });
                    });
            }
        });
    }
}

