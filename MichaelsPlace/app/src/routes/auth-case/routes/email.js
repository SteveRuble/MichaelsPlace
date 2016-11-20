﻿import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {Router} from 'aurelia-router';

@inject(Api, Router)
export class Email {
    constructor(api, router) {
        this.api = api;
        this.router = router;
    }

    sendEmail() {
        var email = this;

        this.api.email.sendToStaff(this.subject, this.message)
            .then(function() {
                email.router.navigateToRoute('program');
            });
    }
}