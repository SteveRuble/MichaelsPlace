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

    removeUser(userId) {
        for (var i = this.organization.people.length; i--;) {
            if (this.organization.people[i].id === userId) {
                this.organization.people.splice(i, 1);
            }
        }

        return this.api.organizations.removeUser(userId, this.organizationId)
            .then(result => this.result = result);
    }
}