import {inject} from 'aurelia-framework';
import {Api} from 'services/api';

@inject(Api)
export class Article {

    constructor(api) {
        this.api = api;
    }

    activate(params) {
        return this.api.articles.getById(params.itemId)
            .then(article => {
                this.title = article.title;
                this.content = article.content;
            });
    }
}