import {inject, NewInstance} from 'aurelia-framework';
import {Api} from 'services/api';
import {log} from 'services/log';
import {Router} from 'aurelia-router';
import {ValidationController, ValidationRules} from 'aurelia-validation';

@inject(Api, Router, NewInstance.of(ValidationController))
export class Email {
    constructor(api, router, validationController) {
        this.api = api;
        this.router = router;
        this.validationController = validationController;

        ValidationRules
            .ensure('subject').required()
            .ensure('message').required()
            .on(Email);
    }

    /**
     * Sends the email to Michael's Place staff and displays the 'program' route.
     */
    sendEmail() {
        this.validationController.validate().then(errors => {
            if (errors.length === 0) {
                var email = this;

                this.api.email.sendToStaff(this.subject, this.message)
                    .then(function() {
                        log.debug('Email sent succesfully. Rerouting to program page.');
                        email.router.navigateToRoute('program');
                    });
            }
        });
    }
}