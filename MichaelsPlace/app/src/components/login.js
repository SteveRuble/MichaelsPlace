import {
    inject
} from 'aurelia-framework';
import {
    User
} from 'models/user';

@inject(User)
export class Login {
    user;
    constructor(user) {
        this.user = user;
    }
    activate() {
        return this.user.update();
    }
}