import {inject} from 'aurelia-framework';
import {Api} from 'services/api';

@inject(Api)
export class ToDo {

    constructor(api) {
        this.api = api;
    }

    activate(params) {
        return this.api.todos.getById(params.itemId)
            .then(todo => {
                this.title = todo.title;
                this.content = todo.content;
            });
    }
}