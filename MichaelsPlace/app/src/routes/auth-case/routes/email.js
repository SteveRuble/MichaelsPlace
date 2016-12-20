﻿import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {log} from 'services/log';
import {Router} from 'aurelia-router';

@inject(Api, Router)
export class Email {
    constructor(api, router) {
        this.api = api;
        this.router = router;
    }

    /**
     * Sends the email to Michael's Place staff and displays the 'program' route.
     */
    sendEmail() {
        var email = this;

        this.api.email.sendToStaff(this.subject, this.message)
            .then(function() {
                log.debug('Email sent succesfully. Rerouting to program page.');
                email.router.navigateToRoute('program');
            });
    }
}