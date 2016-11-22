import {inject, bindable} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {Confirm} from 'components/confirm';

@inject(DialogService, Confirm)
export class Remove {
    @bindable
    action=()=>{};

    @bindable
    msg = "Are you sure";

    constructor(dialogService) {
        this.dialogService = dialogService;
    }

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