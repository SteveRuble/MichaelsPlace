import {inject} from 'aurelia-framework';
import {Api} from 'services/api';
import {User} from 'models/user';

@inject(Api, User)
export class Relationships {

    constructor(api, user) {
        this.heading = 'Who are you?';
        this.api = api;
        this.user = user;
    }

    activate(params) {
        this.user.update();

        this.contextId = params.contextId;
        this.lossId = params.lossId;
        return this.api.tags.getRelationshipTags(params.contextId)
            .then(tags => this.tags = tags);
    }
}