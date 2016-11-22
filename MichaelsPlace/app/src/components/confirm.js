import {inject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';

@inject(DialogController)
export class Confirm {
    
    constructor(dialogController) {
        this.dialogController = dialogController;
    }

    activate(params) {
        this.message = params;
    }
}