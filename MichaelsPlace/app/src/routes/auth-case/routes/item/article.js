import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {log} from 'services/log';
import {Dashboard} from '../../dashboard';
import {User} from 'models/user';

@inject(Api, Dashboard, User)
export class Article {

    constructor(api, dashboard, user) {
        this.api = api;
        this.dashboard = dashboard;
        this.user = user;
    }

    activate(params) {
        this.updateArticle(params.articleId);

        return this.api.articles.getById(params.itemId)
            .then(article => {
                this.title = article.title;
                this.content = article.content;
            });
    }

    /**
     * This method sets the status of the PersonCaseItem (the article) to 'Viewed' on both the front and back ends.
     * @param {} id 
     */
    updateArticle(id) {
        var thisArticle = this;
        var users = thisArticle.dashboard.currentCase.caseUsers.filter(function(u) {
            return u.userId == thisArticle.user.id;
        });

        var article = users[0].articles.filter(function(i) {
            return i.id == id;
        });

        if (article[0].status !== 'Viewed') {
            this.api.articles.updateStatus(id)
                .then(function(result) {
                    if (result.isSuccess) {
                        log.debug('Setting status of article "' + article[0].itemTitle + '" to "Viewed."');
                        article[0].status = 'Viewed';
                    }
                });
        }
    }
}