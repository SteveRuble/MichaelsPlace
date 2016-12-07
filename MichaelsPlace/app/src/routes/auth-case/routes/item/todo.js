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
        var thisTodo = this;
        var currentTodo = this.dashboard.currentCase.todos.filter(function(t) {
            return t.id == thisTodo.todoId;
        });

        this.status = currentTodo[0].status === 'Closed';

        return this.api.todos.getById(params.itemId)
            .then(todo => {
                this.title = todo.title;
                this.content = todo.content;
            });
    }

    /**
     * This method toggles the status of a todo on both the front and back end.
     * @param {} status 
     */
    updateTodo(status) {
        var thisTodo = this;
        this.api.todos.updateStatus(this.todoId, status, this.dashboard.currentCase.id)
            .then(function(result) {
                if (result.isSuccess) {
                    var todo = thisTodo.dashboard.currentCase.todos.filter(function(t) {
                        return t.id == thisTodo.todoId;
                    });

                    if (status) {
                        todo[0].status = "Closed";
                    } else {
                        todo[0].status = "Open";
                    }
                }
            });
    }
}