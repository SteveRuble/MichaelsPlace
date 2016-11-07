export class ArticleFormatValueConverter {
    toView(value) {
        if (value === 'Viewed') {
            return true;
        } else {
            return false;
        }
    }
}