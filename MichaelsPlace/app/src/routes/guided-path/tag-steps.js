import {
    inject
} from 'aurelia-framework';
import {
    Api
} from 'services/api';

@inject(Api)
export class TagSteps {
    heading = 'Describe Yourself';
    tags = [];
    context;
    loss;
    relationship;
    api;

    constructor(api) {
        this.api = api;
    }

    activate(params) {

        return this.api.tags.getContextTags()
            .then(tags => this.tags = tags);
    }
}