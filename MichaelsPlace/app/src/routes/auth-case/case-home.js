import {inject} from 'aurelia-framework';
import {activationStrategy} from 'aurelia-router';
import {Router} from 'aurelia-router';
import {Api} from 'services/api';
import {log} from 'services/log';
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
            log.debug('User is not logged in, rerouting to home page.');
            return this.router.navigate('');
        }

        return this.update();
    }

    hasCases() {
        var count = 0;
        this.cases.forEach(function(i) {
            if (!i.isClosed) {
                count++;
            }
        });
        return count > 0;
    }

    isClosed(isClosed) {
        return isClosed;
    }

    isOrgCase(organizationId) {
        return organizationId;
    }

    update() {
        return this.api.cases.getByPerson()
            .then(cases => this.cases = cases);
    }
}