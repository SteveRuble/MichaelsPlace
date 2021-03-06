﻿import {inject, bindable} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {Confirm} from 'components/confirm';

@inject(DialogService, Confirm)
export class Remove {
    @bindable
    action=()=>{};

    @bindable
    msg = "Are you sure";

    @bindable
    lbl = "";

    constructor(dialogService) {
        this.dialogService = dialogService;
    }

    /**
     * If the user confirms the dialog, then we perform the passed in action, otherwise we return.
     */
    do() {
        this.dialogService.open({
            viewModel: Confirm,
            model: this.msg
        }).then(result => {
            if( result.wasCancelled) return;
            this.action();
        })
    }
}