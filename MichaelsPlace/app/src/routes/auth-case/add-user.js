import {inject} from 'aurelia-framework';
import {Api} from 'services/api';

@inject(Api)
export class AddUser {

    constructor(api) {
        this.api = api;
        this.newUser = '';
        this.additionalUsers = [];
    }

    activate(params) {
        this.caseId = params.caseId;
    }

    sendInvitations() {
        var addresses = [];
        addresses.push(this.newUser);
        for (var i = 0; i < this.additionalUsers.length; i++) {
            addresses.push(this.additionalUsers[i]);
        }

        this.api.email.sendCaseInvitations(addresses, this.caseId)
            .then(function(result) {
                if (result) {
                    alert("Invitations sent.");
                } else {
                    alert("Error sending invitations.");
                }
            });
    }

    addUser() {
        this.additionalUsers.push('');
    }

    removeUser(i) {
        if (this.additionalUsers.length === 1) {
            this.additionalUsers = [];
        } else {
            this.additionalUsers.splice(i, 1); 
        }
    }
}