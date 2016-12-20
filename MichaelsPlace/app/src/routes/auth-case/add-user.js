import {inject, NewInstance} from 'aurelia-framework';
import {Api} from 'services/api';
import {log} from 'services/log';
import {ValidationController, ValidationRules} from 'aurelia-validation';

@inject(Api, NewInstance.of(ValidationController))
export class AddUser {

    constructor(api, validationController) {
        this.api = api;
        this.newUser = '';
        this.additionalUsers = [];
        this.validationController = validationController;

        ValidationRules
            .ensure('newUser').email().required()
            .on(AddUser);
    }

    activate(params) {
        this.caseId = params.caseId;
    }

    sendInvitations() {
        this.validationController.validate().then(errors => {
            if (errors.length === 0) {
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
                            log.debug('Error sending invitations.');
                            alert("Error sending invitations.");
                        }
                    });
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