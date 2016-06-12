(function ($, _, window) {

    // configure underscore templating to use {{ ... }} delimiters
    _.templateSettings = {
        interpolate: /\{\{(.+?)\}\}/g
    };

    jQuery.validator.setDefaults({
        highlight: function (element, errorClass, validClass) {
            if (element.type === 'radio') {
                this.findByName(element.name).addClass(errorClass).removeClass(validClass);
            } else {
                $(element).addClass(errorClass).removeClass(validClass);
                $(element).closest('.control-group').removeClass('success').addClass('error');
            }
        },
        unhighlight: function (element, errorClass, validClass) {
            if (element.type === 'radio') {
                this.findByName(element.name).removeClass(errorClass).addClass(validClass);
            } else {
                $(element).removeClass(errorClass).addClass(validClass);
                $(element).closest('.control-group').removeClass('error').addClass('success');
            }
        }
    });

})($,_,window);