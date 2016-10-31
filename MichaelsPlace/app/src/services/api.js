import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';

@inject(HttpClient)
export class Api {
    http;
    constructor(http) {
        this.http = http;
        this.articles = new ItemApi(http, "articles");
        this.todos = new ItemApi(http, "todos");
        this.user = {
            claims: () => http.fetch('user/claims').then(r => r.json())
        };
        this.tags = new TagApi(http);
        this.cases = new CaseApi(http);
    }
}

export class CaseApi {
    constructor(http) {
        this._http = http;
    }

    /**
     * Gets all the cases for the logged in user.
     * @returns Promise 
     */
    getByPerson() {
        return this._http.fetch(`case/getCases`).then(response => response.json());
    }

    /**
     * Creates a new case for the user based on the situation.
     * @param {} situation 
     * @returns Promise 
     */
    createCase(situation, title) {
        return this._http.fetch(`case/create/${situation}/${title}`).then(response => response.json());
    }
}

export class ItemApi {
    /**
     * Gets the item with the provided id.
     * @param {int} id
     * @returns Promise<Item>
     */
    getById(id) {
            return this._http.fetch(`browsing/${this._itemType}/${id}`).then(response => response.json());
        }
        /**
         * Gets all the items which match the provided situation.
         * @param {Situation} situation
         * @returns Promise<Item[]>
         */
    getBySituation(situation) {
            return this._http.fetch(`browsing/${this._itemType}/${situation}`).then(response => response.json())
        }

        /**
         * Creates an instance of ItemApi.
         * 
         * @param {HttpClient} http
         * @param {string} itemType
         */
    constructor(http, itemType) {
        this._http = http;
        this._itemType = itemType;
    }
}

export class TagApi {
    /**
     * Gets the Context tags.
     * @returns Promise
     */
    getContextTags() {
        return this._http.fetch(`tags/context`).then(response => response.json());
    }

    /**
     * Gets the Loss tags.
     * @param {int} contextId
     * @returns Promise
     */
    getLossTags(contextId = null) {
        return this._http.fetch(`tags/loss/?contextId=${contextId}`).then(response => response.json());
    }

    /**
     * Gets the Relationship tags.
     * @param {int} contextId
     * @returns Promise
     */
    getRelationshipTags(contextId = null) {
            return this._http.fetch(`tags/relationship/?contextId=${contextId}`).then(response => response.json());
        }
        /**
         * Creates an instance of TagApi.
         * 
         * @param {HttpClient} http
         * @param {string} itemType
         */
    constructor(http) {
        this._http = http;
    }
}