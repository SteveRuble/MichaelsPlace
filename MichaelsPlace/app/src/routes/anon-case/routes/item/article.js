import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {log} from "services/log";

@inject(Api)
export class Article {

    constructor(api) {
        this.api = api;
    }

    activate(params) {
        return this.api.articles.getById(params.id)
            .then(article => {
                this.title = article.title;
                this.content = article.content;
                log.debug('Retrieved article with title: "' + this.title + '" and content: "' + this.content + '"');
            });
    }
}