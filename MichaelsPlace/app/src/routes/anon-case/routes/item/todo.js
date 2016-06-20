import {
    inject
} from 'aurelia-framework';
import {
    HttpClient
} from 'aurelia-fetch-client';

@inject(HttpClient)
export class ToDo {
    title;
    content;

    constructor(http) {
        this.http = http;
    }

    activate(params) {
        return this.http.fetch(`browsing/todo/${params.id}`).then(response => response.json())
            .then(article => {
                this.title = article.title;
                this.content = article.content;
            });
    }
}