import {inject} from 'aurelia-framework';
import {Router, activationStrategy} from 'aurelia-router';
import {Api} from 'services/api';
import {User} from 'models/user';

@inject(Api, Router, User)

export class Dashboard {

    constructor(api, router, user) {
        this.api = api;
        this.router = router;
        this.user = user;
    }

    activate(params) {
        this.organizationId = params.organizationId;
    }

    update() {
        
    }
}