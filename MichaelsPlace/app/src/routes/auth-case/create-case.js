import {inject, NewInstance} from 'aurelia-framework';
import {Api} from 'services/api';
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
    }

    /**
     * Creates a case with the specified information and redirects to the case dashboard.
     */
    createCase() {
        this.validationController.validate().then(errors => {
            if (errors.length === 0) {
                var page = this;
        
                this.api.cases.createCase(this.situation, this.title)
                    .then(function(caseId) {
                        if (!caseId) {
                            caseId = -1;
                        }

                        page.router.navigateToRoute('dashboard', { caseId: caseId });
                    });
            }
        });
    }
}