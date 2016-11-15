import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';

@inject(HttpClient)
export class Api {

    constructor(http) {
        this.http = http;
        this.articles = new ItemApi(http, "articles");
        this.todos = new ItemApi(http, "todos");
        this.user = {
            claims: () => http.fetch('user/claims').then(r => r.json())
        };
        this.tags = new TagApi(http);
        this.cases = new CaseApi(http);
        this.email = new EmailApi(http);
        this.organizations = new OrganizationApi(http);
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
        var payload = {
            situation: situation,
            title: title
        }

        return this._http.fetch(`case/create`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(payload)
        }).then(response => response.json());
    }

    /**
     * Gets the case information based on the caseId.
     * @param {} caseId 
     * @returns Promise
     */
    getCase(caseId) {
        return this._http.fetch(`case/getCase/${caseId}`).then(response => response.json());
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
     * Toggles an item's status (CaseItemUserStatus for articles, CaseItemStatus for todos)
     * @param {} id the item's id
     * @param {} status boolean value containing the status to be toggled to
     * @param {} caseId the id of the case
     * @returns {} 
     */
    updateStatus(id, status = true, caseId = '-1') {
        var payload = {
            id: id,
            status: status,
            caseId: caseId
        }

        return this._http.fetch(`item/${this._itemType}/updateStatus`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(payload)
        }).then(response => response.json());
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

export class EmailApi {
    constructor(http) {
        this._http = http;
    }

    sendToStaff(subject, message) {
        var payload = {
            subject: subject,
            message: message
        };

        return this._http.fetch(`email/sendToStaff`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(payload)
        }).then(response => response.json());
    }
}

export class OrganizationApi {
    constructor(http) {
        this._http = http;
    }

    getByPerson() {
        return this._http.fetch(`organization/getOrganizations`).then(response => response.json());
    }

    createOrganization(title, phone, fax, line1, line2, city, state, zip, notes) {
        var payload = {
            name: title,
            phoneNumber: phone,
            faxNumber: fax,
            line1: line1,
            line2: line2,
            city: city,
            state: state,
            zip: zip,
            notes: notes
        }

        return this._http.fetch(`organization/create`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(payload)
        }).then(response => response.json());
    }
}
