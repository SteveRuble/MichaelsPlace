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
                    config.title = 'Aurelia';
                    config.map([{
                        route: ['', 'welcome'],
                        name: 'welcome',
                        moduleId: 'welcome',
                        nav: true,
                        title: 'Welcome'
                    }, {
                        route: 'tag-steps',
                        name: 'tag-steps',
                        moduleId: 'routes/guided-path/context',
                        nav: true,
                        title: 'Guided Path'
                    }, {
                        route: 'tag-steps/losses/:contextId',
                        name: 'losses',
                        moduleId: 'routes/guided-path/losses'
                    }, {
                        route: 'tag-steps/relationships/:contextId/:lossId',
                        name: 'relationships',
                        moduleId: 'routes/guided-path/relationships'
                    }, {
                        route: 'anon-case/:contextId/:lossId/:relationshipId',
                        name: 'anon-case',
                        moduleId: 'routes/anon-case/index'
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
