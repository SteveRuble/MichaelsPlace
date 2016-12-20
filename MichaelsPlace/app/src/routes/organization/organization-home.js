import {inject} from 'aurelia-framework';
import {activationStrategy} from 'aurelia-router';
import {Router} from 'aurelia-router';
import {Api} from 'services/api';
import {log} from 'services/log';
import {User} from 'models/user';

@inject(Router, Api, User)
export class OrganizationHome {

    constructor(router, api, user) {
        this.router = router;
        this.api = api;
        this.user = user;
    }

    activate() {
        this.user.update();
        if (this.user.id == null) {
            log.error('No user logged in, rerouting to home page.');
            return this.router.navigate('');
        }

        return this.update();
    }

    hasOrganizations() {
        return this.organizations.length > 0;
    }

    update() {
        return this.api.organizations.getByPerson()
            .then(organizations => this.organizations = organizations);
    }
}