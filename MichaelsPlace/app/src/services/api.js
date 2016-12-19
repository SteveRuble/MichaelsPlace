import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {log} from "services/log";

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
        log.debug('Getting all cases for the current user...');
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

        log.debug('Creating case with title: "' + title + '" and situation: ' + situation);
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
        log.debug('Retrieving case details for case "' + caseId + '"');
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

        log.debug('Creating case with title: "' + title + '" and situation: ' + situation + 'for organization: ' + organizationId);
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
        log.debug('Deleting case "' + caseId + '"');
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
        log.debug('Closing case "' + caseId + '"');
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

        log.debug('Removing user "' + userId + '" from case "' + caseId + '"');
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
     * @param {HttpClient} http
     * @param {string} itemType
     */
    constructor(http, itemType) {
        this._http = http;
        this._itemType = itemType;
    }

    /**
     * Gets the item with the provided id.
     * @param {int} id
     * @returns Promise<Item>
     */
    getById(id) {
        log.debug('Retrieving ' + this._itemType + ' ' + id);
        return this._http.fetch(`browsing/${this._itemType}/${id}`).then(response => response.json());
    }

    /**
     * Gets all the items which match the provided situation.
     * @param {Situation} situation
     * @returns Promise<Item[]>
     */
    getBySituation(situation) {
        log.debug('Retrieving all ' + this._itemType + ' with situation: ' + situation);
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

        log.debug('Updating ' + this._itemType + ' ' + id + ' with status: ' + status + ' on case "' + caseId + '"');
        return this._http.fetch(`item/${this._itemType}/updateStatus`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(payload)
        }).then(response => response.json());
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
        log.debug('Retrieving all context tags...');
        return this._http.fetch(`tags/context`).then(response => response.json());
    }

    /**
     * Gets the Loss tags.
     * @param {int} contextId
     * @returns Promise
     */
    getLossTags(contextId = null) {
        log.debug('Retrieving all loss tags based on context ' + contextId);
        return this._http.fetch(`tags/loss/?contextId=${contextId}`).then(response => response.json());
    }

    /**
     * Gets the Relationship tags.
     * @param {int} contextId
     * @returns Promise
     */
    getRelationshipTags(contextId = null) {
        log.debug('Retrieving all relationship tags based on context ' + contextId);
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

        log.debug('Sending email to staff with subject: ' + subject + ' and message: ' + message);
        return this._http.fetch(`email/sendToStaff`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(payload)
        }).then(response => response.json());
    }

    sendCaseInvitations(addresses, caseId) {
        var payload = {
            addresses: addresses,
            caseId: caseId
        }

        log.debug('Sending invitations for case "' + caseId + '" to ' + addresses);
        return this._http.fetch(`email/sendCaseInvitations`, {
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
        log.debug('Getting all orgnizations for the current user...');
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
        
        log.debug('Creating organization with title: "' + title + '"');
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
        
        log.debug('Updating organization with title: "' + title + '"');
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
        log.debug('Retrieving organization details for organization ' + organizationId);
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
        
        log.debug('Removing user "' + userId + '" from organization ' + organizationId);
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
        log.debug('Deleting organization ' + organizationId);
        return this._http.fetch(`organization/delete`, {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: json(organizationId)
        }).then(response => response.json());
    }
}
