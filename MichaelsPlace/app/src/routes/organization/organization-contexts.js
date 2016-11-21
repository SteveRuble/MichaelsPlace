import {inject} from 'aurelia-framework';
import {Api} from 'services/api';

@inject(Api)
export class OrganizationContexts {

    constructor(api) {
        this.heading = 'What type of loss occured?';
        this.api = api;
    }

    activate(params) {
        this.organizationId = params.organizationId;

        return this.api.tags.getContextTags()
            .then(tags => this.tags = tags);
    }
}