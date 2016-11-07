import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {Dashboard} from '../../dashboard';

@inject(Api, Dashboard)
export class ToDo {

    constructor(api, dashboard) {
        this.api = api;
        this.dashboard = dashboard;
    }

    activate(params) {
        this.todoId = params.todo;
        return this.api.todos.getById(params.itemId)
            .then(todo => {
                this.title = todo.title;
                this.content = todo.content;
            });
    }

    isTodoClosed() {
        var thisTodo = this;
        var todo = this.dashboard.currentCase.todos.filter(function(t) {
            return t.id == thisTodo.todoId;
        });

        return todo[0].status === 'Closed';
    }

    updateTodo(status) {
        this.api.todos.updateStatus(this.todoId, status, this.dashboard.currentCase.id);
    }
}