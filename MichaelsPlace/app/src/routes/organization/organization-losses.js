import {inject} from 'aurelia-framework';
import {Api} from 'services/api';

@inject(Api)
export class OrganizationLosses {

    constructor(api) {
        this.heading = 'Who was lost?';
        this.api = api;
    }

    activate(params) {
        this.contextId = params.contextId;
        return this.api.tags.getLossTags(params.contextId)
            .then(tags => this.tags = tags);
    }
}