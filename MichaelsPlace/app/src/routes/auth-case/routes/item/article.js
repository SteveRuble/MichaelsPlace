import {inject} from 'aurelia-framework';
import {Api} from 'services/api';

@inject(Api)
export class Article {

    constructor(api) {
        this.api = api;
    }

    activate(params) {
        this.updateArticle(params.articleId, params.viewed);

        return this.api.articles.getById(params.itemId)
            .then(article => {
                this.title = article.title;
                this.content = article.content;
            });
    }

    updateArticle(id, viewed) {
        if (viewed === 'false') {
            this.api.articles.updateStatus(id);
        }
    }
}