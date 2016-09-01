import {inject} from 'aurelia-framework';
import {Api} from 'services/api';

@inject(Api)
export class Losses {

    constructor(api) {
        this.heading = 'Describe your relationship';
        this.api = api;
    }

    activate(params) {
        this.contextId = params.contextId;
        this.lossId = params.lossId;
        return this.api.tags.getRelationshipTags(params.contextId)
            .then(tags => this.tags = tags);
    }
}