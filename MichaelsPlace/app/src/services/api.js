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
    
    /**
     * @param {} http 
     */
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
     * Creates a new case for the user based on the situation and title.
     * @param {} situation 
     * @param {} title
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

    /**
     * Creates a new case for an organization based on the situation and title.
     * @param {} situation  
     * @param {} title
     * @param {} organization
     * @returns Promise
     */
    createOrganizationCase(situation, title, organizationId) {
        var payload = {
            situation: situation,
            title: title,
            organizationId: organizationId
        }

        return this._http.fetch(`case/createOrganizationCase`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(payload)
        }).then(response => response.json());
    }

    /**
     * Deletes the specified case.
     * @param {} caseId 
     * @returns {} 
     */
    deleteCase(caseId) {
        return this._http.fetch(`case/delete`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(caseId)
        }).then(response => response.json());
    }

    /**
     * Closes the specified case.
     * @param {} caseId 
     * @returns {} 
     */
    closeCase(caseId) {
        return this._http.fetch(`case/close`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(caseId)
        }).then(response => response.json());
    }

    /**
     * Removes the specified user from the specified case.
     * @param {} userId 
     * @param {} caseId
     */
    removeUser(userId, caseId) {
        var payload = {
            userId: userId,
            caseId: caseId
        }

        return this._http.fetch(`case/removeUser`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(payload)
        }).then(response => response.json());
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
     * @param {HttpClient} http
     * @param {string} itemType
     */
    constructor(http) {
        this._http = http;
    }

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
}

export class EmailApi {

    /**
     * @param {} http
     */
    constructor(http) {
        this._http = http;
    }

    /**
     * Sends an email message to Michael's Place staff.
     * @param {} subject 
     * @param {} message 
     */
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

    /**
     * @param {} http 
     */
    constructor(http) {
        this._http = http;
    }

    /**
     * Gets the logged in user's organizations.
     */
    getByPerson() {
        return this._http.fetch(`organization/getOrganizations`).then(response => response.json());
    }

    /**
     * Creates a new organization based on the specified information.
     * @param {} title 
     * @param {} phone 
     * @param {} fax 
     * @param {} line1 
     * @param {} line2 
     * @param {} city 
     * @param {} state 
     * @param {} zip 
     * @param {} notes 
     */
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

    /**
     * Updates an organization with the specified information.
     * @param {} title 
     * @param {} phone 
     * @param {} fax 
     * @param {} line1 
     * @param {} line2 
     * @param {} city 
     * @param {} state 
     * @param {} zip 
     * @param {} notes 
     * @param {} organization 
     * @returns {} 
     */
    editOrganization(title, phone, fax, line1, line2, city, state, zip, notes, organizationId) {
        var payload = {
            name: title,
            phoneNumber: phone,
            faxNumber: fax,
            line1: line1,
            line2: line2,
            city: city,
            state: state,
            zip: zip,
            notes: notes,
            organizationId: organizationId
        }

        return this._http.fetch(`organization/edit`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(payload)
        }).then(response => response.json());
    }

    /**
     * Gets organization information based on the organizationId.
     * @param {} organizationId 
     * @returns Promise
     */
    getOrganization(organizationId) {
        return this._http.fetch(`organization/getOrganization/${organizationId}`).then(response => response.json());
    }

    /**
     * Removes the specified user from the specified organization.
     * @param {} userId 
     * @param {} organizationId
     */
    removeUser(userId, organizationId) {
        var payload = {
            userId: userId,
            organizationId: organizationId
        }

        return this._http.fetch(`organization/removeUser`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(payload)
        }).then(response => response.json());
    }

    /**
     * Deletes the specified organization.
     * @param {} organizationId 
     */
    deleteOrganization(organizationId) {
        return this._http.fetch(`organization/delete`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(organizationId)
        }).then(response => response.json());
    }
}
