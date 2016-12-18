import {inject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {log} from "services/log";

@inject(DialogController)
export class Confirm {
    
    constructor(dialogController) {
        this.dialogController = dialogController;
    }

    activate(params) {
        this.message = params;
        log.debug('Creating dialog with message: \"' + this.message + '\"');
    }
}