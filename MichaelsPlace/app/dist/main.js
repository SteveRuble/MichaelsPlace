'use strict';

System.register(['bootstrap', 'aurelia-fetch-client'], function (_export, _context) {
    "use strict";

    var HttpClient;


    function configureHttpClient(container) {
        var httpClient = new HttpClient();
        httpClient.configure(function (config) {
            config.withBaseUrl('http://localhost:8080/api/').withDefaults({
                credentials: 'same-origin',
                headers: {
                    'Accept': 'application/json',
                    'X-Requested-With': 'Fetch'
                }
            }).withInterceptor({
                request: function request(_request) {
                    console.debug('→ ' + _request.method + ' ' + _request.url);
                    return _request;
                },
                response: function response(_response) {
                    console.debug('← ' + _response.status + ' : ' + _response.url);
                    return _response;
                }
            });
        });

        container.registerInstance(HttpClient, httpClient);
    }
    return {
        setters: [function (_bootstrap) {}, function (_aureliaFetchClient) {
            HttpClient = _aureliaFetchClient.HttpClient;
        }],
        execute: function () {
            function configure(aurelia) {
                aurelia.use.standardConfiguration().developmentLogging();

                aurelia.use.plugin('aurelia-animator-css');


                configureHttpClient(aurelia.container);
                aurelia.start().then(function () {
                    return aurelia.setRoot();
                });
            }
            _export('configure', configure);
        }
    };
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm1haW4uanMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7QUFtQkEsYUFBUyxtQkFBVCxDQUE2QixTQUE3QixFQUF1QztBQUNuQyxZQUFJLGFBQWEsSUFBSSxVQUFKLEVBQWpCO0FBQ0EsbUJBQVcsU0FBWCxDQUFxQixrQkFBVTtBQUMzQixtQkFDSyxXQURMLENBQ2lCLDRCQURqQixFQUVLLFlBRkwsQ0FFa0I7QUFDViw2QkFBYSxhQURIO0FBRVYseUJBQVM7QUFDTCw4QkFBVSxrQkFETDtBQUVMLHdDQUFvQjtBQUZmO0FBRkMsYUFGbEIsRUFTSyxlQVRMLENBU3FCO0FBQ2IsdUJBRGEsbUJBQ0wsUUFESyxFQUNJO0FBQ2IsNEJBQVEsS0FBUixRQUFtQixTQUFRLE1BQTNCLFNBQXFDLFNBQVEsR0FBN0M7QUFDQSwyQkFBTyxRQUFQO0FBQ0gsaUJBSlk7QUFLYix3QkFMYSxvQkFLSixTQUxJLEVBS007QUFDZiw0QkFBUSxLQUFSLFFBQW1CLFVBQVMsTUFBNUIsV0FBd0MsVUFBUyxHQUFqRDtBQUNBLDJCQUFPLFNBQVA7QUFDSDtBQVJZLGFBVHJCO0FBbUJILFNBcEJEOztBQXNCQSxrQkFBVSxnQkFBVixDQUEyQixVQUEzQixFQUF1QyxVQUF2QztBQUNIOzs7QUEzQ1Esc0IsdUJBQUEsVTs7O0FBRUYscUJBQVMsU0FBVCxDQUFtQixPQUFuQixFQUE0QjtBQUMvQix3QkFBUSxHQUFSLENBQ0sscUJBREwsR0FFSyxrQkFGTDs7QUFLQSx3QkFBUSxHQUFSLENBQVksTUFBWixDQUFtQixzQkFBbkI7OztBQU1BLG9DQUFvQixRQUFRLFNBQTVCO0FBQ0Esd0JBQVEsS0FBUixHQUFnQixJQUFoQixDQUFxQjtBQUFBLDJCQUFNLFFBQVEsT0FBUixFQUFOO0FBQUEsaUJBQXJCO0FBQ0giLCJmaWxlIjoibWFpbi5qcyIsInNvdXJjZVJvb3QiOiIvc3JjIn0=
