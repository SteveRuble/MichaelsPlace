(function ($, _, window) {

    // configure underscore templating to use {{ ... }} delimiters
    _.templateSettings = {
        interpolate: /\{\{(.+?)\}\}/g
    };

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "5000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    $(function() {
        $(document)
            .ajaxError(function(e, jqXHR, ajaxSettings, error) {
                toastr.error("AJAX error: <a href='/elmah.axd'>" + error + "</a>");
            })
        .ajaxSend(function() {
                $("#ajax-spinner").show();
        })
        .ajaxComplete(function() {
                $("#ajax-spinner").hide();
                
            });
    });



})($,_,window);