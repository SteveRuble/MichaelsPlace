import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {log} from "services/log";

@inject(Api)
export class ToDo {

    constructor(api) {
        this.api = api;
    }

    activate(params) {
        return this.api.todos.getById(params.id)
            .then(todo => {
                this.title = todo.title;
                this.content = todo.content;
                log.debug('Retrieved todo with title: "' + this.title + '" and content: "' + this.content + '"');
            });
    }
}