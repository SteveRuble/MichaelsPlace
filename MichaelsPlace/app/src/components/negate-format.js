export class NegateFormatValueConverter {

    /**
     * The purpose of this converter is simply to negate the passed in value.
     * @param {} value 
     */
    toView(value) {
        return !value;
    }
}