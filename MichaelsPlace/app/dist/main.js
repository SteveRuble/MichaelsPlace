'use strict';

System.register(['bootstrap', 'aurelia-fetch-client', 'models/user'], function (_export, _context) {
    "use strict";

    var HttpClient, User;


    function configureHttpClient(container) {
        var httpClient = new HttpClient();
        httpClient.configure(function (config) {
            console.dir(config);
            config.withBaseUrl('http://localhost:8080/api/').withDefaults({
                credentials: 'include',
                headers: {
                    'Accept': 'application/json',
                    'X-Requested-With': 'Fetch'
                }
            }).withInterceptor({
                request: function request(_request) {
                    console.debug('-> ' + _request.method + ' ' + _request.url);
                    return _request;
                },
                response: function response(_response) {
                    console.debug('<- ' + _response.status + ' : ' + _response.url);
                    return _response;
                }
            });
        });

        container.registerInstance(HttpClient, httpClient);
        return httpClient;
    }

    function configureUser(container, httpClient) {
        var user = new User(httpClient);
        container.registerInstance(User, user);
        return user;
    }
    return {
        setters: [function (_bootstrap) {}, function (_aureliaFetchClient) {
            HttpClient = _aureliaFetchClient.HttpClient;
        }, function (_modelsUser) {
            User = _modelsUser.User;
        }],
        execute: function () {
            function configure(aurelia) {
                aurelia.use.standardConfiguration().developmentLogging();

                aurelia.use.plugin('aurelia-animator-css');


                var httpClient = configureHttpClient(aurelia.container);
                var user = configureUser(aurelia.container, httpClient);

                user.update().then(function () {
                    return aurelia.start();
                }).then(function () {
                    return aurelia.setRoot();
                });
            }
            _export('configure', configure);
        }
    };
});
//# sourceMappingURL=main.js.map
