export class ArticleFormatValueConverter {

    toView(value) {
        return value !== 'Viewed';
    }
}