import {
    inject
} from 'aurelia-framework';
import {
    HttpClient
} from 'aurelia-fetch-client';

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