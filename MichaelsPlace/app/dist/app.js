'use strict';

System.register(['aurelia-framework', 'models/user'], function (_export, _context) {
    "use strict";

    var inject, User, _dec, _class, App;

    function _classCallCheck(instance, Constructor) {
        if (!(instance instanceof Constructor)) {
            throw new TypeError("Cannot call a class as a function");
        }
    }

    return {
        setters: [function (_aureliaFramework) {
            inject = _aureliaFramework.inject;
        }, function (_modelsUser) {
            User = _modelsUser.User;
        }],
        execute: function () {
            _export('App', App = (_dec = inject(User), _dec(_class = function () {
                function App(user) {
                    _classCallCheck(this, App);

                    this.user = user;
                }

                App.prototype.activate = function activate() {
                    return this.user.update();
                };

                App.prototype.configureRouter = function configureRouter(config, router) {
                    config.title = "Michael's Place";
                    config.map([{
                        route: ['', 'home'],
                        name: 'home',
                        moduleId: 'routes/guided-path/home',
                        nav: true,
                        title: 'Home'
                    }, {
                        route: 'tag-steps/context',
                        name: 'contexts',
                        moduleId: 'routes/guided-path/contexts',
                        title: 'Guided Path'
                    }, {
                        route: 'tag-steps/losses/:contextId',
                        name: 'losses',
                        moduleId: 'routes/guided-path/losses',
                        title: 'Guided Path'
                    }, {
                        route: 'tag-steps/relationships/:contextId/:lossId',
                        name: 'relationships',
                        moduleId: 'routes/guided-path/relationships',
                        title: 'Guided Path'
                    }, {
                        route: 'anon-case/:contextId/:lossId/:relationshipId',
                        name: 'anon-case',
                        moduleId: 'routes/anon-case/index',
                        title: 'Anonymous Case'
                    }, {
                        route: 'auth-case/case-home',
                        name: 'case-home',
                        moduleId: 'routes/auth-case/case-home',
                        title: 'Case Home'
                    }, {
                        route: 'auth-case/create-case/:situation',
                        name: 'create-case',
                        moduleId: 'routes/auth-case/create-case',
                        title: 'Create Case'
                    }, {
                        route: 'auth-case/dashboard/:caseId',
                        name: 'dashboard',
                        moduleId: 'routes/auth-case/dashboard',
                        title: 'Case Dashboard'
                    }, {
                        route: 'organization/organization-home',
                        name: 'organization-home',
                        moduleId: 'routes/organization/organization-home',
                        title: 'Organization Home'
                    }, {
                        route: 'organization/create-organization',
                        name: 'create-organization',
                        moduleId: 'routes/organization/create-organization',
                        title: 'Create Organization'
                    }, {
                        route: 'organization/organization-contexts',
                        name: 'organization-contexts',
                        moduleId: 'routes/organization/organization-contexts',
                        title: 'Guided Path'
                    }, {
                        route: 'organization/organization-losses/:contextId',
                        name: 'organization-losses',
                        moduleId: 'routes/organization/organization-losses',
                        title: 'Guided Path'
                    }, {
                        route: 'organization/organization-relationships/:contextId/:lossId',
                        name: 'organization-relationships',
                        moduleId: 'routes/organization/organization-relationships',
                        title: 'Guided Path'
                    }, {
                        route: 'organization/create-organization-case/:situation',
                        name: 'create-organization-case',
                        moduleId: 'routes/organization/create-organization-case',
                        title: 'Create Organization Case'
                    }, {
                        route: 'organization/organization-dashboard/:organizationId',
                        name: 'organization-dashboard',
                        moduleId: 'routes/organization/organization-dashboard',
                        title: 'Organization Dashboard'
                    }]);

                    this.router = router;
                };

                return App;
            }()) || _class));

            _export('App', App);
        }
    };
});
//# sourceMappingURL=app.js.map
