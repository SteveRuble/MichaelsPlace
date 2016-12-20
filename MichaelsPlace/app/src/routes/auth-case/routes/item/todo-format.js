export class TodoFormatValueConverter {

    toView(value) {
        return value === 'Closed';
    }
}