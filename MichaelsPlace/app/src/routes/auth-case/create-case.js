import {inject, NewInstance} from 'aurelia-framework';
import {Api} from 'services/api';
import {log} from 'services/log';
import {Router} from 'aurelia-router';
import {ValidationController, ValidationRules} from 'aurelia-validation';

@inject(Api, Router, NewInstance.of(ValidationController))
export class CreateCase {

    constructor(api, router, validationController) {
        this.api = api;
        this.router = router;
        this.validationController = validationController;

        ValidationRules
            .ensure('title').required()
            .on(CreateCase);
    }

    activate(params) {
        this.situation = params.situation;
        this.organizationId = params.organizationId;
    }

    create() {
        if (this.organizationId > 0) {
            return this.createOrganizationCase();
        } else {
            return this.createCase();
        }
    }

    createOrganizationCase() {
        this.validationController.validate().then(errors => {
            if (errors.length === 0) {
                var page = this;
        
                this.api.cases.createOrganizationCase(this.situation, this.title, this.organizationId)
                    .then(function(caseId) {
                        if (!caseId) {
                            log.error('Error: Unable to create organization case.');
                        }

                        page.router.navigateToRoute('organization-dashboard', { organizationId: page.organizationId });
                    });
                }
            });
    }

    createCase() {
        this.validationController.validate().then(errors => {
            if (errors.length === 0) {
                var page = this;
        
                this.api.cases.createCase(this.situation, this.title)
                    .then(function(caseId) {
                        if (!caseId) {
                            log.dubg('Error creating case.');
                            caseId = -1;
                        }

                        page.router.navigateToRoute('dashboard', { caseId: caseId });
                    });
            }
        });
    }
}