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
        if (this.cases.length > 0) {
            this.hasCases = true;
        } else {
            this.hasCases = false;
        }
    }

    update() {
        var caseHome = this;
        return this.api.cases.getByPerson()
            .then(function(cases) {
                caseHome.cases = cases;
                caseHome.hasCases();
            });
    }
}