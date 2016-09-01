import {
    inject
} from 'aurelia-framework';
import {
    User
} from 'models/user';

@inject(User)
export class App {
    user;
    constructor(user) {
        this.user = user;
    }
    activate() {
        return this.user.update();
    }
    configureRouter(config, router) {
        config.title = 'Aurelia';
        config.map([{
            route: ['', 'welcome'],
            name: 'welcome',
            moduleId: 'welcome',
            nav: true,
            title: 'Welcome'
        }, {
            route: 'tag-steps',
            name: 'tag-steps',
            moduleId: 'routes/guided-path/context',
            nav: true,
            title: 'Guided Path'
        }, {
            route: 'tag-steps/losses/:contextId',
            name: 'losses',
            moduleId: 'routes/guided-path/losses'
        }, {
            route: 'tag-steps/relationships/:contextId/:lossId',
            name: 'relationships',
            moduleId: 'routes/guided-path/relationships'
        }, {
            route: 'anon-case/:contextId/:lossId/:relationshipId',
            name: 'anon-case',
            moduleId: 'routes/anon-case/index'
        }]);

        this.router = router;
    }
}