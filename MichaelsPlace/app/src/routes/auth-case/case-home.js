import {inject} from 'aurelia-framework';
import {activationStrategy} from 'aurelia-router';
import {Router} from 'aurelia-router';
import {Api} from 'services/api';
import {User} from 'models/user';

@inject(Router, Api, User)
export class CaseHome {

    constructor(router, api, user) {
        this.router = router;
        this.api = api;
        this.user = user;
    }

    activate() {
        this.user.update();
        if (this.user.id == null) {
            return this.router.navigate('');
        }

        return this.update();
    }

    hasCases() {
        return this.cases.length > 0;
    }

    update() {
        return this.api.cases.getByPerson()
            .then(cases => this.cases = cases);
    }
}