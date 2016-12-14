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
        
    }
}