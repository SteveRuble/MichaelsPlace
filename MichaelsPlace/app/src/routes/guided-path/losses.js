﻿import {inject} from 'aurelia-framework';
import {Api} from 'services/api';

@inject(Api)
export class Losses {

    constructor(api) {
        this.heading = 'Describe your loss';
        this.api = api;
    }

    activate(params) {
        this.contextId = params.contextId;
        return this.api.tags.getLossTags(params.contextId)
            .then(tags => this.tags = tags);
    }
}