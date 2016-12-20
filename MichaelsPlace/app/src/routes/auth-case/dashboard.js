import {inject} from 'aurelia-framework';
import {Router, activationStrategy} from 'aurelia-router';
import {Api} from 'services/api';
import {log} from 'services/log';
import {User} from 'models/user';

@inject(Api, Router, User)

export class Dashboard {

    constructor(api, router, user) {
        this.api = api;
        this.router = router;
        this.user = user;
    }

    configureRouter(config, router) {
        config.map(
            [{
                route: ['', 'program'],
                name: 'program',
                moduleId: 'routes/auth-case/program',
                activationStrategy: activationStrategy.replace
            }, {
                route: 'article/:itemId/:articleId/',
                name: 'article',
                moduleId: 'routes/auth-case/routes/item/article',
                activationStrategy: activationStrategy.replace
            }, {
                route: 'todo/:itemId/:todo/:owner',
                name: 'todo',
                moduleId: 'routes/auth-case/routes/item/todo',
                activationStrategy: activationStrategy.replace
            }, {
                route: 'email',
                name: 'email',
                moduleId: 'routes/auth-case/routes/email',
                activationStrategy: activationStrategy.replace
            }]);

        this.router = router;
    }

    activate(params) {
        this.caseId = params.caseId;
        return this.update();
    }

    update() {
        var dashboard = this;

        return this.api.cases.getCase(this.caseId)
            .then(function(myCase) {
                if (!myCase.id) {
                    log.debug('Failed to retrieve case details, rerouting to case-home.');
                    dashboard.router.navigateToRoute('case-home');
                }

                dashboard.currentCase = myCase;
                
                var users = dashboard.currentCase.caseUsers.filter(function(u) {
                    return u.userId == dashboard.user.id;
                });

                dashboard.currentCase.articles = users[0].articles;
            });
    }

    isOrganizationCase() {
        if (this.currentCase.organizationId) {
            return true;
        } else {
            this.currentCase.organizationId = -1;
            return false;
        }
    }

    isCaseOwner() {
        var owner = this.currentCase.caseUsers.filter(function(u) {
            return u.caseOwner;
        });

        if (owner.length <= 0) {
            return false;
        }

        return owner[0].userId === this.user.id;
    }

    closeCase() {
        var dashboard = this;

        return this.api.cases.closeCase(this.caseId)
            .then(function(caseId) {
                if (caseId) {
                    dashboard.router.navigateToRoute('case-home');
                } else {
                    log.debug('Unable to close case.');
                    alert("Error: Unable to close case.");
                }
            });
    }

    removeUser(userId) {
        for (var i = this.currentCase.caseUsers.length; i--;) {
            if (this.currentCase.caseUsers[i].id === userId) {
                this.currentCase.caseUsers.splice(i, 1);
            }
        }

        return this.api.cases.removeUser(userId, this.caseId)
            .then(result => this.result = result);
    }
}