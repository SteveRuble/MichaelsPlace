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
                route: 'article/:itemId/:articleId/:viewed',
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

    updateArticle(id, status) {
        this.api.articles.updateStatus(id, status);
    }

    isTodoClosed(id) {
        var todo = this.currentCase.todos.filter(function(t) {
            return t.id === id;
        });

        return todo[0].status === 'Closed';
    }

    isArticleViewed(id) {
        var dashboard = this;

        var users = this.currentCase.caseUsers.filter(function(u) {
            return u.userId == dashboard.user.id;
        });

        var article = users[0].personItems.filter(function(i) {
            return i.id == id;
        });

        return article[0].status === 'Viewed';
    }
}