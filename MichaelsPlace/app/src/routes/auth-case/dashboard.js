import {inject} from 'aurelia-framework';
import {Router, activationStrategy} from 'aurelia-router';
import {Api} from 'services/api';

@inject(Api, Router)

export class Dashboard {

    constructor(api, router) {
        this.api = api;
        this.router = router;
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
                route: 'todo/:itemId/:todo',
                name: 'todo',
                moduleId: 'routes/auth-case/routes/item/todo',
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
            });
    }
}