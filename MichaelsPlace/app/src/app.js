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
            moduleId: 'routes/guided-path/tag-steps',
            nav: true,
            title: 'Guided Path'
        }, {
            route: 'anon-case',
            name: 'anon-case',
            moduleId: 'routes/anon-case/index',
            nav: true,
            title: 'Anonymous Case'
        }]);

        this.router = router;
    }
}