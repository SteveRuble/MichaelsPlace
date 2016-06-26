import {
    inject
} from 'aurelia-framework';
import {
    Api
} from 'services/api';

@inject(Api)
export class Article {
    title;
    content;

    constructor(api) {
        this.api = api;
    }

    activate(params) {
        return this.api.articles.getById(params.id)
            .then(article => {
                this.title = article.title;
                this.content = article.content;
            });
    }
}