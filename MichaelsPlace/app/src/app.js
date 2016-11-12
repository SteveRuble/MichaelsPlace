import {inject} from 'aurelia-framework';
import {User} from 'models/user';

@inject(User)
export class App {

    constructor(user) {
        this.user = user;
    }

    activate() {
        return this.user.update();
    }

    configureRouter(config, router) {
        config.title = "Michael's Place";
        config.map([{
            route: ['', 'home'],
            name: 'tag-steps',
            moduleId: 'routes/guided-path/home',
            nav: true,
            title: 'Home'
        }, {
            route: 'tag-steps/context',
            name: 'contexts',
            moduleId: 'routes/guided-path/contexts',
            title: 'Guided Path'
        }, {
            route: 'tag-steps/losses/:contextId',
            name: 'losses',
            moduleId: 'routes/guided-path/losses',
            title: 'Guided Path'
        }, {
            route: 'tag-steps/relationships/:contextId/:lossId',
            name: 'relationships',
            moduleId: 'routes/guided-path/relationships',
            title: 'Guided Path'
        }, {
            route: 'anon-case/:contextId/:lossId/:relationshipId',
            name: 'anon-case',
            moduleId: 'routes/anon-case/index',
            title: 'Anonymous Case'
        }, {
            route: 'auth-case/case-home',
            name: 'case-home',
            moduleId: 'routes/auth-case/case-home',
        }, {
            route: 'auth-case/create-case/:situation',
            name: 'create-case',
            moduleId: 'routes/auth-case/create-case'
        }, {
            route: 'auth-case/dashboard/:caseId',
            name: 'dashboard',
            moduleId: 'routes/auth-case/dashboard'
        }, {
            route: 'organization/organization-home',
            name: 'organization-home',
            moduleId: 'routes/organization/organization-home'
        }]);

        this.router = router;
    }
}