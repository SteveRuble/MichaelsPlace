import {
    inject
} from 'aurelia-framework';
import {
    Api
} from 'services/api';
import {
    activationStrategy
} from 'aurelia-router';

@inject(Api)
export class Index {
    heading = 'Describe Yourself';
    situation = '1-5-9';
    articles = [];
    todos = [];
    currentItem = [];

    constructor(api) {
        this.api = api;
    }

    activate() {
        return this.update();
    }

    configureRouter(config, router) {
        config.map(
            [{
                route: ['', 'welcome'],
                name: 'welcome',
                moduleId: 'welcome',
                nav: true,
                title: 'Welcome'
            }, {
                route: 'article/:id',
                name: 'article',
                moduleId: 'routes/anon-case/routes/item/article',
                activationStrategy: activationStrategy.replace
            }, {
                route: 'todo/:id',
                name: 'todo',
                moduleId: 'routes/anon-case/routes/item/todo',
                activationStrategy: activationStrategy.replace
            }]);
        this.router = router;
    }

    update() {
        return this.api.articles.getBySituation(this.situation)
            .then(items => this.articles = items)
            .then(_ => this.api.todos.getBySituation(this.situation))
            .then(items => this.todos = items);
    }

    select(item) {
        this.currentItem = item;
    }
}