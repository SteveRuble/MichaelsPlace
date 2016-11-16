export class ArticleFormatValueConverter {
    toView(value) {
        if (value === 'Viewed') {
            return false;
        } else {
            return true;
        }
    }
}