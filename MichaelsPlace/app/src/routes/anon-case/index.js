import {
    inject
} from 'aurelia-framework';
import {
    HttpClient
} from 'aurelia-fetch-client';
import {
    activationStrategy
} from 'aurelia-router';

@inject(HttpClient)
export class Index {
    heading = 'Describe Yourself';
    situation = '1-5-9';
    articles = [];
    todos = [];
    currentItem = [];

    constructor(http) {
        this.http = http;
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
        return this.http.fetch(`browsing/bysituation/${this.situation}/article`)
            .then(response => response.json())
            .then(items => this.articles = items)
            .then(_ => this.http.fetch(`browsing/bysituation/${this.situation}/todo`))
            .then(response => response.json())
            .then(items => this.todos = items);
    }

    select(item) {
        this.currentItem = item;
    }
}