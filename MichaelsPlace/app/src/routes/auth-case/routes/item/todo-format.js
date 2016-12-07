export class TodoFormatValueConverter {

    toView(value) {
        if (value === 'Closed') {
            return true;
        } else {
            return false;
        }
    }
}