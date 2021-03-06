import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {log} from "services/log";
import {activationStrategy} from 'aurelia-router';

@inject(Api)
export class Index {

    constructor(api) {
        this.api = api;
    }

    activate(params) {
        this.situation = params.contextId + '-' + params.lossId + '-' + params.relationshipId;
        log.debug('Initializing anonymous case dashboard for situation: ' + this.situation);
        return this.update();
    }

    configureRouter(config, router) {
        config.map(
            [{
                route: '',
                name: 'program',
                moduleId: 'routes/anon-case/program',
                activationStrategy: activationStrategy.replace
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
            .then(items => {
                this.articles = items;
                log.debug('Retrieved ' + items.length + ' articles.');
            })
            .then(_ => this.api.todos.getBySituation(this.situation))
            .then(items => {
                this.todos = items;
                log.debug('Retrieved ' + items.length + ' todos.');
            });
    }
}